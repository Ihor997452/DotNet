using DotNet.PlatformService.DTOs;
using DotNet.PlatformService.Model;

namespace DotNet.PlatformService.Profiles;

public class PlatformsProfile : AutoMapper.Profile
{
    public PlatformsProfile()
    {
        // Source -> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishDto>();
    } 
}