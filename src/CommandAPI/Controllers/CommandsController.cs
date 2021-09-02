using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;
using AutoMapper;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace CommandAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandsController : ControllerBase {
	    // random change
        private readonly ICommandAPIRepo _repository;
        private readonly IMapper _mapper;
        public CommandsController(ICommandAPIRepo repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands() {
            var commandItems = _repository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{id}", Name="GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id) {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem == null) {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> 
            CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var CommandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), 
                new {Id = CommandReadDto.Id}, CommandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, 
                CommandUpdateDto commandUpdateDto)
        {
            var commandModelfromRepo = _repository.GetCommandById(id);
            if (commandModelfromRepo == null){
                return NotFound();
            }
            _mapper.Map(commandUpdateDto, commandModelfromRepo);
            _repository.UpdateCommand(commandModelfromRepo);
            _repository.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id,
            JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelfromRepo = _repository.GetCommandById(id);
            if (commandModelfromRepo == null)
            {
                return NotFound();
            }
            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelfromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);
            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(commandToPatch, commandModelfromRepo);
            _repository.UpdateCommand(commandModelfromRepo);
            _repository.SaveChanges();
            return NoContent();
        }
    }       
}
