using AutoMapper;
using DotNet.CommandService.DTOs;
using DotNet.CommandService.Models;

namespace DotNet.CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        // Source -> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<PlatformPublishDto, Platform>()
            .ForMember(dest => dest.ExternalId,
                opt => opt.MapFrom(src => src.Id));
        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalId,
                opt => opt.MapFrom(src => src.PlatformId));
    }
}