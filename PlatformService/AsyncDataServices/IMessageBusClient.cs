using DotNet.PlatformService.DTOs;

namespace DotNet.PlatformService.AsyncDataServices;

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishDto platformPublishDto);
}