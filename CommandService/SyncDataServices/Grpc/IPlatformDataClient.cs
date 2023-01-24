using DotNet.CommandService.Models;

namespace DotNet.CommandService.SyncDataServices.Grpc;

public interface IPlatformDataClient
{
    IEnumerable<Platform>? ReturnAllPlatforms();
}