using AutoMapper;
using DotNet.CommandService.Data;
using DotNet.CommandService.DTOs;
using DotNet.CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.CommandService.Controllers;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepo _repo;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetAllPlatformCommands(int platformId)
    {
        Console.WriteLine($"--> Hit GetAllPlatformCommands: {platformId}");

        if (!_repo.PlatformExists(platformId))
        {
            return NotFound();
        }

        var commands = _repo.GetAllPlatformCommands(platformId);

        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetPlatformCommand")]
    public ActionResult<CommandReadDto> GetPlatformCommand(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit GetPlatformCommand: {platformId} / {commandId}");

        if (!_repo.PlatformExists(platformId))
        {
            return NotFound();
        }

        var command = _repo.GetPlatformCommand(platformId, commandId);
        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommand(int platformId, CommandCreateDto commandCreateDto)
    {
        Console.WriteLine($"--> Hit CreateCommand: {platformId}");
        if (!_repo.PlatformExists(platformId))
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(commandCreateDto);
        _repo.CreateCommand(platformId, command);
        _repo.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);
        return CreatedAtRoute(
            nameof(GetPlatformCommand),
            new { platformId = platformId, commandId = commandReadDto.Id },
            commandReadDto);
    }

}
