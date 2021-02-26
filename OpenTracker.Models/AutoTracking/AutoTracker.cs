﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using WebSocketSharp;

namespace OpenTracker.Models.AutoTracking
{
    /// <summary>
    /// This is the class containing autotracking data and methods
    /// </summary>
    public class AutoTracker : IAutoTracker
    {
        private readonly IMemoryAddress.Factory _addressFactory;
        private byte? _inGameStatus;
        private readonly Dictionary<MemorySegmentType, List<IMemoryAddress>> _memorySegments =
            new Dictionary<MemorySegmentType, List<IMemoryAddress>>();

        private bool CanReadMemory =>
            SNESConnector.Status == ConnectionStatus.Connected;
        private bool IsInGame =>
            _inGameStatus.HasValue && _inGameStatus.Value > 0x05 && _inGameStatus.Value != 0x14 &&
            _inGameStatus.Value < 0x20;

        public Action<LogLevel, string>? LogHandler { get; set; }

        public ISNESConnector SNESConnector { get; }
        public Dictionary<ulong, IMemoryAddress> MemoryAddresses { get; } =
            new Dictionary<ulong, IMemoryAddress>();

        public event PropertyChangedEventHandler? PropertyChanged;

        private IEnumerable<string> _devices = new List<string>();
        public IEnumerable<string> Devices
        {
            get => _devices;
            private set
            {
                if (_devices != value)
                {
                    _devices = value;
                    OnPropertyChanged(nameof(Devices));
                }
            }
        }

        private bool _raceIllegalTracking;
        public bool RaceIllegalTracking
        {
            get => _raceIllegalTracking;
            set
            {
                _raceIllegalTracking = value;
                OnPropertyChanged(nameof(RaceIllegalTracking));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="snesConnector">
        /// The SNES connector.
        /// </param>
        /// <param name="addressFactory">
        /// An Autofac factory for creating memory addresses.
        /// </param>
        public AutoTracker(
            ISNESConnector snesConnector, IMemoryAddress.Factory addressFactory)
        {
            _addressFactory = addressFactory;

            SNESConnector = snesConnector;

            foreach (MemorySegmentType type in Enum.GetValues(typeof(MemorySegmentType)))
            {
                _memorySegments.Add(type, new List<IMemoryAddress>());
            }

            for (ulong i = 0; i < 256; i++)
            {
                CreateMemoryAddress(MemorySegmentType.FirstRoom, i);
                CreateMemoryAddress(MemorySegmentType.SecondRoom, i);

                if (i < 144)
                {
                    CreateMemoryAddress(MemorySegmentType.Item, i);
                }

                if (i < 130)
                {
                    CreateMemoryAddress(MemorySegmentType.OverworldEvent, i);
                }

                if (i < 80)
                {
                    CreateMemoryAddress(MemorySegmentType.ThirdRoom, i);
                }

                if (i < 48)
                {
                    CreateMemoryAddress(MemorySegmentType.Dungeon, i);
                }

                if (i < 6)
                {
                    CreateMemoryAddress(MemorySegmentType.DungeonItem, i);
                }

                if (i < 2)
                {
                    CreateMemoryAddress(MemorySegmentType.NPCItem, i);
                }
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">
        /// The string of the property name of the changed property.
        /// </param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Returns the starting address of the specified memory segment.
        /// </summary>
        /// <param name="type">
        /// The memory segment type.
        /// </param>
        /// <returns>
        /// A 64-bit unsigned integer representing the starting memory address of the segment.
        /// </returns>
        private static ulong GetMemorySegmentStart(MemorySegmentType type)
        {
            return type switch
            {
                MemorySegmentType.FirstRoom => 0x7ef000,
                MemorySegmentType.SecondRoom => 0x7ef100,
                MemorySegmentType.ThirdRoom => 0x7ef200,
                MemorySegmentType.OverworldEvent => 0x7ef280,
                MemorySegmentType.Item => 0x7ef340,
                MemorySegmentType.NPCItem => 0x7ef410,
                MemorySegmentType.DungeonItem => 0x7ef434,
                MemorySegmentType.Dungeon => 0x7ef4c0,
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        /// <summary>
        /// Creates a memory address for the specified memory segment and offset.
        /// </summary>
        /// <param name="type">
        /// The memory segment type.
        /// </param>
        /// <param name="offset">
        /// The offset of the address.
        /// </param>
        private void CreateMemoryAddress(MemorySegmentType type, ulong offset)
        {
            var memoryAddress = _addressFactory();
            var memorySegment = _memorySegments[type];
            memorySegment.Add(memoryAddress);
            var address = GetMemorySegmentStart(type);
            address += offset;
            MemoryAddresses.Add(address, memoryAddress);
        }

        /// <summary>
        /// Updates cached values of a memory segment.
        /// </summary>
        /// <param name="segment">
        /// The segment to be updated.
        /// </param>
        private void MemoryCheck(MemorySegmentType segment)
        {
            var memorySegment = _memorySegments[segment];
            var startAddress = GetMemorySegmentStart(segment);
            byte[] buffer = new byte[memorySegment.Count];

            if (SNESConnector.Read(startAddress, buffer))
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    memorySegment[i].Value = buffer[i];
                }
            }
        }

        /// <summary>
        /// Returns the list of devices provided by the SNES connector.
        /// </summary>
        /// <returns>
        /// A list of strings representing the devices.
        /// </returns>
        private async Task<IEnumerable<string>> GetDevicesFromConnector()
        {
            var devices = await SNESConnector.GetDevices() ??
                new List<string>();

            return devices;
        }

        /// <summary>
        /// Updates cached value of the SNES memory address that provides game status.
        /// </summary>
        public void InGameCheck()
        {
            if (CanReadMemory && SNESConnector.Read(0x7e0010, out byte inGameStatus))
            {
                _inGameStatus = inGameStatus;
            }
        }

        /// <summary>
        /// Updates cached values of all SNES memory addresses.
        /// </summary>
        public void MemoryCheck()
        {
            if (CanReadMemory && IsInGame)
            {
                foreach (MemorySegmentType segment in Enum.GetValues(typeof(MemorySegmentType)))
                {
                    MemoryCheck(segment);
                }
            }
        }

        /// <summary>
        /// Returns whether the web socket can be connected to.
        /// </summary>
        /// <returns>
        /// A boolean representing whether the web socket can be connected to.
        /// </returns>
        public bool CanConnect()
        {
            return SNESConnector.Status == ConnectionStatus.NotConnected;
        }

        /// <summary>
        /// Connects to the web socket with the specified URI string.
        /// </summary>
        /// <param name="uriString">
        /// A string representing the web socket URI.
        /// </param>
        public async Task Connect(string uriString)
        {
            await Task.Factory.StartNew(() =>
            {
                SNESConnector.SetUri(uriString);
                SNESConnector.Connect();
            });
        }

        /// <summary>
        /// Returns whether the web socket is able to provide the devices.
        /// </summary>
        /// <returns>
        /// A boolean representing whether the web socket is able to provide the devices.
        /// </returns>
        public bool CanGetDevices()
        {
            return SNESConnector.Status == ConnectionStatus.SelectDevice;
        }

        /// <summary>
        /// Updates the list of devices.
        /// </summary>
        public async Task GetDevices()
        {
            Devices = await GetDevicesFromConnector();
        }

        /// <summary>
        /// Returns whether the web socket can be disconnected.
        /// </summary>
        /// <returns>
        /// A boolean representing whether the web socket can be disconnected.
        /// </returns>
        public bool CanDisconnect()
        {
            return SNESConnector.Status != ConnectionStatus.NotConnected;
        }

        /// <summary>
        /// Disconnects the autotracker.
        /// </summary>
        public async Task Disconnect()
        {
            await Task.Factory.StartNew(() =>
            {
                SNESConnector.Disconnect();
                _inGameStatus = null;

                foreach (var address in MemoryAddresses.Values)
                {
                    address.Reset();
                }
            });
        }

        /// <summary>
        /// Returns whether autotracking can be started.
        /// </summary>
        /// <returns>
        /// A boolean representing whether autotracking can be started.
        /// </returns>
        public bool CanStart()
        {
            return SNESConnector.Status == ConnectionStatus.SelectDevice;
        }

        /// <summary>
        /// Starts autotracking.
        /// </summary>
        public async Task Start(string device)
        {
            await Task.Factory.StartNew(() =>
            {
                SNESConnector.SetDevice(device);
            });
        }
    }
}
