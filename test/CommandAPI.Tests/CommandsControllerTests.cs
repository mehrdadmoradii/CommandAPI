using System;
using Xunit;
using CommandAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Moq;
using AutoMapper;
using CommandAPI.Data;
using CommandAPI.Models;
using CommandAPI.Profiles;
using CommandAPI.Dtos;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockrepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public CommandsControllerTests()
        {
            mockrepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockrepo = null;
            realProfile = null;
            configuration = null;
            mapper = null;
        }

        [Fact]
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            // Arrange
            mockrepo.Setup(repo =>
            repo.GetAllCommands()).Returns(GetCommands(0));

            var controller = new CommandsController(mockrepo.Object, mapper);

            // Act
            var result = controller.GetAllCommands();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsOneItem_WhenDBHasOneResource()
        {
            // Arrange 
            mockrepo.Setup(repo =>
                repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockrepo.Object, mapper);

            // Act
            var result = controller.GetAllCommands();

            // Assert
            var okresult = result.Result as OkObjectResult;
            var commands = okresult.Value as List<CommandReadDto>;

            Assert.Single(commands);
        }

        [Fact]
        public void GetAllCommands_Returns200OK_WhenDBHasOneResource()
        {
            // Arrange
            mockrepo.Setup(repo =>
                repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockrepo.Object, mapper);

            // Act
            var result = controller.GetAllCommands();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandByID_Returns404NotFound_WhenNonExistentIDProvided()
        {
            // Arrange 
            mockrepo.Setup(repo =>
                repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockrepo.Object, mapper);

            // Act
            var result = controller.GetCommandById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommandByID_Returns200OK__WhenValidIDProvided()
        {
            // Arrange
            mockrepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command { 
                    Id = 1,
                    HowTo = "mock",
                    Platform = "Mock",
                    CommandLine = "Mock" });

            var controller = new CommandsController(mockrepo.Object, mapper);

            // Act
            var result = controller.GetCommandById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
            // Arrange
            mockrepo.Setup(repo 
                => repo.GetCommandById(1)).Returns(new Command { Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock" });

            var controller = new CommandsController(mockrepo.Object, mapper);

            // Act
            var result = controller.CreateCommand(new CommandCreateDto{});

            // Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
        {
            // Arrange
            mockrepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command { Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock" });

            var controller = new CommandsController(mockrepo.Object, mapper);

            // Act
            var result = controller.CreateCommand(new CommandCreateDto{});

            // Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        [Fact]
        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            // Arrange
            mockrepo.Setup(repo => 
                repo.GetCommandById(1)).Returns(new Command { Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock" });

            var controller = new CommandsController(mockrepo.Object, mapper);

            // Act
            var result = controller.UpdateCommand(1, new CommandUpdateDto {});

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            // Arrange
            mockrepo.Setup(repo =>
                repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockrepo.Object, mapper);

            // Act
            var result = controller.UpdateCommand(0, new CommandUpdateDto {});

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            // Arrange
            mockrepo.Setup(repo => 
                repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockrepo.Object, mapper);

            // Act
            var result = controller.PartialCommandUpdate(0,
                new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto>{ });

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0)
            {
                commands.Add(new Command {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
    }
}