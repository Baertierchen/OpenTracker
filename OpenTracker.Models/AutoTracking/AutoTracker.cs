﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTracker.Models.AutoTracking.Memory;
using OpenTracker.Models.AutoTracking.SNESConnectors;
using ReactiveUI;

namespace OpenTracker.Models.AutoTracking
{
    /// <summary>
    ///     This class contains auto-tracking logic and data.
    /// </summary>
    public class AutoTracker : ReactiveObject, IAutoTracker
    {
        private readonly IMemoryAddressProvider _memoryAddressProvider;
        private readonly ISNESConnector _snesConnector;

        private IMemoryAddress InGameStatus { get; }

        private bool CanReadMemory => Status == ConnectionStatus.Connected;
        private bool IsInGame
        {
            get
            {
                var inGameValue = InGameStatus.Value;
                
                if (inGameValue is null)
                {
                    return false;
                }
                
                return inGameValue > 0x05 && inGameValue != 0x14 && inGameValue < 0x20;
            }
        }

        private IList<string> _devices = new List<string>();
        public IList<string> Devices
        {
            get => _devices;
            private set => this.RaiseAndSetIfChanged(ref _devices, value);
        }

        private bool _raceIllegalTracking;
        public bool RaceIllegalTracking
        {
            get => _raceIllegalTracking;
            set => this.RaiseAndSetIfChanged(ref _raceIllegalTracking, value);
        }

        private ConnectionStatus _status;
        public ConnectionStatus Status
        {
            get => _status;
            private set => this.RaiseAndSetIfChanged(ref _status, value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="memoryAddressProvider">
        /// The memory address provider.
        /// </param>
        /// <param name="snesConnector">
        /// The SNES connector factory.
        /// </param>
        public AutoTracker(IMemoryAddressProvider memoryAddressProvider, ISNESConnector snesConnector)
        {
            _memoryAddressProvider = memoryAddressProvider;
            _snesConnector = snesConnector;

            InGameStatus = _memoryAddressProvider.MemoryAddresses[0x7e0010];

            _snesConnector.StatusChanged += OnStatusChanged;
        }

        public bool CanConnect()
        {
            return Status == ConnectionStatus.NotConnected;
        }

        public async Task Connect(string uriString)
        {
            _snesConnector.SetUri(uriString);
            await _snesConnector.Connect();
        }

        public bool CanGetDevices()
        {
            return Status == ConnectionStatus.SelectDevice;
        }

        public async Task GetDevices()
        {
            Devices = await GetDevicesFromConnector();
        }

        public bool CanDisconnect()
        {
            return Status != ConnectionStatus.NotConnected;
        }

        public async Task Disconnect()
        { 
            await _snesConnector.Disconnect();
            _memoryAddressProvider.Reset();
        }

        public bool CanStart()
        {
            return Status == ConnectionStatus.SelectDevice;
        }

        public async Task Start(string device)
        {
            await _snesConnector.SetDevice(device);
        }

        public async Task InGameCheck()
        {
            if (!CanReadMemory)
            {
                return;
            }

            var result = await _snesConnector.Read(0x7e0010);

            if (result is null)
            {
                return;
            }

            InGameStatus.Value = result[0];
        }

        public async Task MemoryCheck()
        {
            if (!CanReadMemory || !IsInGame)
            {
                return;
            }

            foreach (MemorySegmentType type in Enum.GetValues(typeof(MemorySegmentType)))
            {
                await MemoryCheck(type);
            }
        }

        /// <summary>
        ///     Subscribes to the StatusChanged event on the ISNESConnector interface.
        /// </summary>
        /// <param name="sender">
        ///     The sending object of the event.
        /// </param>
        /// <param name="status">
        ///     The arguments of the StatusChanged event.
        /// </param>
        private void OnStatusChanged(object? sender, ConnectionStatus status)
        {
            Status = status;
        }

        /// <summary>
        ///     Returns the list of devices provided by the SNES connector.
        /// </summary>
        /// <returns>
        ///     A list of strings representing the devices.
        /// </returns>
        private async Task<IList<string>> GetDevicesFromConnector()
        {
            var devices = await _snesConnector.GetDevices();

            return devices is null ? new List<string>() : new List<string>(devices);
        }
        
        /// <summary>
        ///     Updates cached values of a memory segment.
        /// </summary>
        /// <param name="segment">
        ///     The segment to be updated.
        /// </param>
        private async Task MemoryCheck(MemorySegmentType segment)
        {
            var memorySegment = _memoryAddressProvider.MemorySegments[segment];
            var startAddress = _memoryAddressProvider.GetMemorySegmentStart(segment);
            var buffer = await _snesConnector.Read(startAddress, memorySegment.Count);

            if (buffer is not null)
            {
                for (var i = 0; i < buffer.Length; i++)
                {
                    memorySegment[i].Value = buffer[i];
                }
            }
        }
    }
}
