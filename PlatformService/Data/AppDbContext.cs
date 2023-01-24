using DotNet.PlatformService.Model;
using Microsoft.EntityFrameworkCore;

namespace DotNet.PlatformService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Platform>? Platforms { get; set; }
}