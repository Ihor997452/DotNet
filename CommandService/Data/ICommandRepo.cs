using DotNet.CommandService.Models;

namespace DotNet.CommandService.Data;

public interface ICommandRepo
{
    bool SaveChanges();

    //Platforms
    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform platform);
    bool PlatformExists(int platformId);
    bool ExternalPlatformExists(int externalPlatformId);
    
    //Commands 
    IEnumerable<Command> GetAllPlatformCommands(int platformId);
    Command GetPlatformCommand(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);
}