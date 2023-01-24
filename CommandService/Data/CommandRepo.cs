using DotNet.CommandService.Models;

namespace DotNet.CommandService.Data;

public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext _context;

    public CommandRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _context.Platforms!.ToList();
    }

    public void CreatePlatform(Platform platform)
    {
        _context.Platforms!.Add(platform);
    }

    public bool PlatformExists(int platformId)
    {
        return _context.Platforms!.Any(p => p.Id == platformId);
    }

    public bool ExternalPlatformExists(int externalPlatformId)
    {
        return _context.Platforms!.Any(p => p.ExternalId == externalPlatformId);
    }

    public IEnumerable<Command> GetAllPlatformCommands(int platformId)
    {
        return _context.Commands!
            .Where(c => c.PlatformId == platformId)
            .OrderBy(c => c.Platform!.Name);
    }

    public Command GetPlatformCommand(int platformId, int commandId)
    { 
        return _context.Commands!
            .Where(c => c.PlatformId == platformId)
            .OrderBy(c => c.Platform!.Name)
            .FirstOrDefault()!;
    }

    public void CreateCommand(int platformId, Command command)
    {
        command.PlatformId = platformId;
        _context.Commands!.Add(command);
    }
}