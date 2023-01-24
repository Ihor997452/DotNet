using DotNet.PlatformService.Model;
using Microsoft.EntityFrameworkCore;

namespace DotNet.PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        
        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
        if (isProduction)
        {
            Migrate(context);
            SeedData(context);
        }
        else
        {
            SeedData(context);
        }
    }

    private static void Migrate(DbContext? context)
    {
        try
        {
            Console.WriteLine("--> Migrating...");
            context?.Database.Migrate(); 
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not run migrations: {e.Message}");
        }
    }
    
    private static void SeedData(AppDbContext? context)
    {
        if (context?.Platforms != null && context.Platforms.Any()) return;
        
        Console.WriteLine("--> Seeding Data");
        context?.Platforms?.AddRange(
            new Platform() {Name = "1", Publisher = "1", Cost = "1"}, 
            new Platform() {Name = "2", Publisher = "2", Cost = "2"}, 
            new Platform() {Name = "3", Publisher = "3", Cost = "3"});
        context?.SaveChanges();
    }
}