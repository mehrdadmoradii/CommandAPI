using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;

namespace CommandAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandsController : ControllerBase {
        private readonly ICommandAPIRepo _repository;
        public CommandsController(ICommandAPIRepo repo)
        {
            _repository = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetAllCommands() {
            var commandItems = _repository.GetAllCommands();
            return Ok(commandItems);
        }

        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandsById(int id) {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem == null) {
                return NotFound();
            }
            return Ok(commandItem);
        }
    }       
}