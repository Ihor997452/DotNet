using Microsoft.EntityFrameworkCore;

namespace DotNet.ProductCatalogService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
        if (isProduction)
        {
            Migrate(context);
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
}