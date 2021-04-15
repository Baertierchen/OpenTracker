using System;
using System.ComponentModel;
using Autofac;
using NSubstitute;
using OpenTracker.Models.Accessibility;
using OpenTracker.Models.Modes;
using OpenTracker.Models.Requirements;
using OpenTracker.Models.Requirements.BossShuffle;
using Xunit;

namespace OpenTracker.UnitTests.Models.Requirements.BossShuffle
{
    public class BossShuffleRequirementTests
    {
        private readonly IMode _mode = Substitute.For<IMode>();

        [Fact]
        public void ModeChanged_ShouldUpdateMetValue()
        {
            var sut = new BossShuffleRequirement(_mode, true);
            _mode.BossShuffle.Returns(true);

            _mode.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(
                _mode, new PropertyChangedEventArgs(nameof(IMode.BossShuffle)));
            
            Assert.True(sut.Met);
        }

        [Fact]
        public void Met_ShouldRaisePropertyChanged()
        {
            var sut = new BossShuffleRequirement(_mode, true);
            _mode.BossShuffle.Returns(true);

            Assert.PropertyChanged(sut, nameof(IRequirement.Met), 
                () => _mode.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(
                    _mode, new PropertyChangedEventArgs(nameof(IMode.BossShuffle))));
        }

        [Fact]
        public void Met_ShouldRaiseChangePropagated()
        {
            var sut = new BossShuffleRequirement(_mode, true);
            _mode.BossShuffle.Returns(true);

            var eventRaised = false;

            void Handler(object? sender, EventArgs e)
            {
                eventRaised = true;
            }
            
            sut.ChangePropagated += Handler;
            _mode.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(
                _mode, new PropertyChangedEventArgs(nameof(IMode.BossShuffle)));
            sut.ChangePropagated -= Handler;
            
            Assert.True(eventRaised);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, false, true)]
        [InlineData(true, true, true)]
        public void Met_ShouldReturnExpectedValue(bool expected, bool bossShuffle, bool requirement)
        {
            _mode.BossShuffle.Returns(bossShuffle);
            var sut = new BossShuffleRequirement(_mode, requirement);
            
            Assert.Equal(expected, sut.Met);
        }

        [Fact]
        public void Accessibility_ShouldRaisePropertyChanged()
        {
            var sut = new BossShuffleRequirement(_mode, true);
            _mode.BossShuffle.Returns(true);

            Assert.PropertyChanged(sut, nameof(IRequirement.Accessibility),
                () => _mode.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(
                    _mode, new PropertyChangedEventArgs(nameof(IMode.BossShuffle))));
        }

        [Theory]
        [InlineData(AccessibilityLevel.Normal, false, false)]
        [InlineData(AccessibilityLevel.None, false, true)]
        [InlineData(AccessibilityLevel.Normal, true, true)]
        public void Accessibility_ShouldReturnExpectedValue(
            AccessibilityLevel expected, bool bigKeyShuffle, bool requirement)
        {
            _mode.BossShuffle.Returns(bigKeyShuffle);
            var sut = new BossShuffleRequirement(_mode, requirement);
            
            Assert.Equal(expected, sut.Accessibility);
        }
        
        [Fact]
        public void AutofacTest()
        {
            using var scope = ContainerConfig.Configure().BeginLifetimeScope();
            var factory = scope.Resolve<IBossShuffleRequirement.Factory>();
            var sut = factory(false);
            
            Assert.NotNull(sut as BossShuffleRequirement);
        }
    }
}