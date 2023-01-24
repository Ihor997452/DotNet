using DotNet.CommandService.Models;
using DotNet.CommandService.SyncDataServices.Grpc;

namespace DotNet.CommandService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
        var platforms = grpcClient!.ReturnAllPlatforms();
        
        SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
    }

    private static void SeedData(ICommandRepo? repo, IEnumerable<Platform>? platforms)
    {
        Console.WriteLine("--> Seeding new platforms...");
        
        if (platforms == null) return;
        
        foreach (var platform in platforms)
        {
            if (repo != null && !repo.ExternalPlatformExists(platform.ExternalId))
            {
                repo.CreatePlatform(platform);
            }

            repo?.SaveChanges();
        }
    }
}