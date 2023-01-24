using DotNet.PlatformService.DTOs;

namespace DotNet.PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
    {
        Console.WriteLine(_configuration["CommandService"]);
        var response = await _httpClient.PostAsync(_configuration["CommandService"], null);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "--> Sync POST to CommandService was OK"
            : "--> Sync POST to CommandService wa NOT OK");
    }
}