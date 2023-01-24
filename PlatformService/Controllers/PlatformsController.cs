using AutoMapper;
using DotNet.PlatformService.AsyncDataServices;
using DotNet.PlatformService.Data;
using DotNet.PlatformService.DTOs;
using DotNet.PlatformService.Model;
using DotNet.PlatformService.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repo;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(
        IPlatformRepo repo, 
        IMapper mapper, 
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient)
    {
        _repo = repo;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var platforms = _repo.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpGet("{id:int}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var platform = _repo.GetPlatformById(id);
        return platform == null ? NotFound() : Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platformModel = _mapper.Map<Platform>(platformCreateDto);
        _repo.CreatePlatform(platformModel);
        _repo.SaveChanges();
        var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
        
        //Send Sync Message
        await SendPlatformToCommandSync(platformReadDto);
        
        //Send Async Message
        SendPlatformToCommandAsync(platformReadDto);

        return CreatedAtRoute(nameof(GetPlatformById), new { platformReadDto.Id }, platformReadDto);
    }

    private void SendPlatformToCommandAsync(PlatformReadDto platformReadDto)
    {
        try
        {
            var platformPublishDto = _mapper.Map<PlatformPublishDto>(platformReadDto);
            platformPublishDto.Event = "Platform.Published";
            _messageBusClient.PublishNewPlatform(platformPublishDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not send asynchronously: {e.Message}");
        }
    }
    
    private async Task SendPlatformToCommandSync(PlatformReadDto platformReadDto)
    {
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not send synchronously: {e.Message}");
        }
    }
}