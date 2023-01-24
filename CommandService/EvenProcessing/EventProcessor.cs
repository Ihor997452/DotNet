using System.Text.Json;
using AutoMapper;
using DotNet.CommandService.Data;
using DotNet.CommandService.DTOs;
using DotNet.CommandService.Models;

namespace DotNet.CommandService.EvenProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor (
        IServiceScopeFactory scopeFactory,
        IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string message)
    {
        Console.WriteLine($"--> Determining Event Type of: {message}");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

        switch (eventType?.Event)
        {
            case "Platform.Published":
                Console.WriteLine("--> Platform Publish Event Detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("--> Could Not Determine Event Type");
                return EventType.Undetermined;
        }
    }

    private void AddPlatform(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
        var platformPublishDto = JsonSerializer.Deserialize<PlatformPublishDto>(message);

        try
        {
            var platform = _mapper.Map<Platform>(platformPublishDto);

            if (!repo.ExternalPlatformExists(platform.ExternalId))
            {
                repo.CreatePlatform(platform);
                repo.SaveChanges();
                Console.WriteLine("--> Platform Added!");
            }
            else
            {
                Console.WriteLine("--> Platform Already Exists!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could Not Add Platform To DB: {e.Message}");
        }
    }
}