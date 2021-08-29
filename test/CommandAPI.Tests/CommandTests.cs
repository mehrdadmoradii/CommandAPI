using System;
using Xunit;
using CommandAPI.Models;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command testCommand;

        public CommandTests()
        {
            testCommand = new Command
            {
                HowTo = "Do something",
                Platform = "Some platform",
                CommandLine = "Some commandline"
            };
        }

        public void Dispose()
        {
            testCommand = null;
        }

        [Fact]
        public void CanChangeHowTo()
        {
            // Arrange
            var testCommand = new Command 
            {
                HowTo = "Do something awsome",
                Platform = ".Net Core",
                CommandLine = "dotnet test"
            };

            // Act
            testCommand.HowTo = "execute unit tests";

            // Assert 
            Assert.Equal("execute unit tests", testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlatform()
        {
            // Arrange

            // Act
            testCommand.Platform = "Xunit";

            // Assert
            Assert.Equal("Xunit", testCommand.Platform);
        }

        [Fact]
        public void CanChangeCommandLine()
        {
            // Arrange

            // Act
            testCommand.CommandLine = "dotnet test";

            // Assert
            Assert.Equal("dotnet test", testCommand.CommandLine);
        }
    }
}