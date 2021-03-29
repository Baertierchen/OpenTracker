using System;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using OpenTracker.Models.AutoTracking;
using OpenTracker.Models.AutoTracking.Memory;
using OpenTracker.Models.AutoTracking.SNESConnectors;
using Xunit;

namespace OpenTracker.UnitTests.Models.AutoTracking
{
    public class AutoTrackerTests
    {
        private readonly ISNESConnector _snesConnector;
        private readonly IMemoryAddressProvider _memoryAddressProvider =
            new MemoryAddressProvider(() => new MemoryAddress());
        
        private readonly AutoTracker _sut;

        public AutoTrackerTests()
        {
            _snesConnector = Substitute.For<ISNESConnector>();
            _sut = new AutoTracker(_memoryAddressProvider, _snesConnector);
        }
        
        [Theory]
        [InlineData(ConnectionStatus.NotConnected, ConnectionStatus.NotConnected)]
        [InlineData(ConnectionStatus.Connecting, ConnectionStatus.Connecting)]
        [InlineData(ConnectionStatus.SelectDevice, ConnectionStatus.SelectDevice)]
        [InlineData(ConnectionStatus.Attaching, ConnectionStatus.Attaching)]
        [InlineData(ConnectionStatus.Connected, ConnectionStatus.Connected)]
        [InlineData(ConnectionStatus.Error, ConnectionStatus.Error)]
        public void StatusChanged_ShouldBeEqualToExpected(ConnectionStatus expected, ConnectionStatus eventValue)
        {
            _snesConnector.StatusChanged += Raise.Event<EventHandler<ConnectionStatus>>(
                _snesConnector, eventValue);
            
            Assert.Equal(expected, _sut.Status);
        }
        
        [Fact]
        public async void Devices_ShouldReturnEmptyList_WhenNullReceivedFromConnector()
        {
            _snesConnector.GetDevices().ReturnsNull();

            await _sut.GetDevices();
            
            Assert.Empty(_sut.Devices);
        }

        [Fact]
        public async void Devices_ShouldReturnReceivedList()
        {
            _snesConnector.GetDevices().Returns(new[]
            {
                "Test 1",
                "Test 2",
                "Test 3"
            });

            await _sut.GetDevices();
            
            Assert.Collection(_sut.Devices,
                item => Assert.Equal("Test 1", item),
                item => Assert.Equal("Test 2", item),
                item => Assert.Equal("Test 3", item));
        }

        [Fact]
        public async void Devices_ShouldRaisePropertyChanged_WhenValueChanges()
        {
            _snesConnector.GetDevices().Returns(new[]
            {
                "Test 1",
                "Test 2",
                "Test 3"
            });

            await Assert.PropertyChangedAsync(_sut, nameof(IAutoTracker.Devices), () => _sut.GetDevices());
        }

        [Fact]
        public void RaceIllegalTracking_ShouldRaisePropertyChanged_WhenValueChanges()
        {
            Assert.PropertyChanged(
                _sut, nameof(IAutoTracker.RaceIllegalTracking), () => _sut.RaceIllegalTracking = true);
        }

        [Fact]
        public void Status_ShouldRaisePropertyChanged_WhenValueChanges()
        {
            Assert.PropertyChanged(
                _sut, nameof(IAutoTracker.Status), () =>
                    _snesConnector.StatusChanged += Raise.Event<EventHandler<ConnectionStatus>>(
                        _snesConnector, ConnectionStatus.Connected));
        }

        [Theory]
        [InlineData(true, ConnectionStatus.NotConnected)]
        [InlineData(false, ConnectionStatus.Connecting)]
        [InlineData(false, ConnectionStatus.SelectDevice)]
        [InlineData(false, ConnectionStatus.Attaching)]
        [InlineData(false, ConnectionStatus.Connected)]
        [InlineData(false, ConnectionStatus.Error)]
        public void CanConnect_ShouldReturnExpected(bool expected, ConnectionStatus status)
        {
            _snesConnector.StatusChanged += Raise.Event<EventHandler<ConnectionStatus>>(
                _snesConnector, status);
            
            Assert.Equal(expected, _sut.CanConnect());
        }

        [Fact]
        public async void Connect_ShouldCallSetUriOnSNESConnector()
        {
            const string uriString = "Test";
            await _sut.Connect(uriString);
            
            _snesConnector.Received().SetUri(uriString);
        }

        [Fact]
        public async void Connect_ShouldCallConnectOnSNESConnector()
        {
            const string uriString = "Test";
            await _sut.Connect(uriString);

            await _snesConnector.Received().Connect();
        }

        [Theory]
        [InlineData(false, ConnectionStatus.NotConnected)]
        [InlineData(false, ConnectionStatus.Connecting)]
        [InlineData(true, ConnectionStatus.SelectDevice)]
        [InlineData(false, ConnectionStatus.Attaching)]
        [InlineData(false, ConnectionStatus.Connected)]
        [InlineData(false, ConnectionStatus.Error)]
        public void CanGetDevices_ShouldReturnExpected(bool expected, ConnectionStatus status)
        {
            _snesConnector.StatusChanged += Raise.Event<EventHandler<ConnectionStatus>>(
                _snesConnector, status);
            
            Assert.Equal(expected, _sut.CanGetDevices());
        }

        [Fact]
        public async void GetDevices_ShouldCallGetDevicesOnSNESConnector()
        {
            await _sut.GetDevices();

            await _snesConnector.Received().GetDevices();
        }

        [Theory]
        [InlineData(false, ConnectionStatus.NotConnected)]
        [InlineData(true, ConnectionStatus.Connecting)]
        [InlineData(true, ConnectionStatus.SelectDevice)]
        [InlineData(true, ConnectionStatus.Attaching)]
        [InlineData(true, ConnectionStatus.Connected)]
        [InlineData(true, ConnectionStatus.Error)]
        public void CanDisconnect_ShouldReturnExpected(bool expected, ConnectionStatus status)
        {
            _snesConnector.StatusChanged += Raise.Event<EventHandler<ConnectionStatus>>(
                _snesConnector, status);
            
            Assert.Equal(expected, _sut.CanDisconnect());
        }

        [Fact]
        public async void Disconnect_ShouldCallDisconnectOnSNESConnector()
        {
            await _sut.Disconnect();

            await _snesConnector.Received().Disconnect();
        }
        
        [Theory]
        [InlineData(false, ConnectionStatus.NotConnected)]
        [InlineData(false, ConnectionStatus.Connecting)]
        [InlineData(true, ConnectionStatus.SelectDevice)]
        [InlineData(false, ConnectionStatus.Attaching)]
        [InlineData(false, ConnectionStatus.Connected)]
        [InlineData(false, ConnectionStatus.Error)]
        public void CanStart_ShouldReturnExpected(bool expected, ConnectionStatus status)
        {
            _snesConnector.StatusChanged += Raise.Event<EventHandler<ConnectionStatus>>(
                _snesConnector, status);
            
            Assert.Equal(expected, _sut.CanStart());
        }

        [Fact]
        public async void Start_ShouldCallSetDeviceOnSNESConnector()
        {
            const string device = "Test";
            await _sut.Start(device);

            await _snesConnector.Received().SetDevice(device);
        }

        [Fact]
        public async void InGameCheck_ShouldDoNothing_WhenCanReadMemoryReturnsFalse()
        {
            await _sut.InGameCheck();
            
            await _snesConnector.DidNotReceive().Read(Arg.Any<ulong>());
        }

        [Fact]
        public async void InGameCheck_ShouldCallRead_WhenCanReadMemoryReturnsTrue()
        {
            _snesConnector.StatusChanged += Raise.Event<EventHandler<ConnectionStatus>>(
                _snesConnector, ConnectionStatus.Connected);
            _snesConnector.Read(0x7e0010).ReturnsNull();
            await _sut.InGameCheck();

            await _snesConnector.Received().Read(0x7e0010);
        }

        [Fact]
        public async void InGameCheck_ShouldSetInGameStatusValue_WhenReadMemoryReturnsTrue()
        {
            const byte value = 0x07;
            _snesConnector.StatusChanged += Raise.Event<EventHandler<ConnectionStatus>>(
                _snesConnector, ConnectionStatus.Connected);
            _snesConnector.Read(0x7e0010).Returns(new[] {value});
            await _sut.InGameCheck();
            
            Assert.Equal(value, _memoryAddressProvider.MemoryAddresses[0x7e0010].Value);
        }

        [Fact]
        public async void MemoryCheck_ShouldDoNothing_WhenCanReadMemoryReturnsFalse()
        {
            await _sut.InGameCheck();
            
            await _snesConnector.DidNotReceive().Read(Arg.Any<ulong>(), Arg.Any<int>());
        }

        [Fact]
        public async void MemoryCheck_ShouldDoNothing_WhenIsInGameReturnsFalse()
        {
            _snesConnector.StatusChanged += Raise.Event<EventHandler<ConnectionStatus>>(
                _snesConnector, ConnectionStatus.Connected);
            _snesConnector.Read(0x7e0010).ReturnsNull();
            await _sut.InGameCheck();
            await _sut.MemoryCheck();
            
            await _snesConnector.Received(1).Read(Arg.Any<ulong>(), Arg.Any<int>());
        }

        [Fact]
        public async void MemoryCheck_ShouldCallReadOnSNESConnector_WhenCanReadMemoryAndIsInGameReturnTrue()
        {
            _snesConnector.StatusChanged += Raise.Event<EventHandler<ConnectionStatus>>(
                _snesConnector, ConnectionStatus.Connected);
            _snesConnector.Read(Arg.Any<ulong>(), Arg.Any<int>()).Returns(x =>
            {
                var bytesToRead = (int)x[1];
                
                var result = new byte[bytesToRead];

                for (var i = 0; i < bytesToRead; i++)
                {
                    result[i] = 0x7;
                }

                return result;
            });
            await _sut.InGameCheck();
            await _sut.MemoryCheck();

            await _snesConnector.Received(Enum.GetValues(typeof(MemorySegmentType)).Length + 1).Read(
                Arg.Any<ulong>(), Arg.Any<int>());
        }
    }
}