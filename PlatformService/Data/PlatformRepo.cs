using DotNet.PlatformService.Model;

namespace DotNet.PlatformService.Data;

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _context;

    public PlatformRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public bool SaveChanges()
    {
        return _context.SaveChanges().Equals(1);
    }

    public IEnumerable<Platform?> GetAllPlatforms()
    {
        return _context.Platforms != null ? _context.Platforms.ToList() : new();
    }

    public Platform? GetPlatformById(int id)
    {
        return _context.Platforms?.FirstOrDefault(platform => platform.Id == id);
    }

    public void CreatePlatform(Platform? platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _context.Platforms?.Add(platform);
    }
}