using System.Text;
using System.Text.Json;
using DotNet.PlatformService.DTOs;
using RabbitMQ.Client;

namespace DotNet.PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection = null!;
    private readonly IModel _channel = null!;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;

        var factory = CreateFactory();

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            Console.WriteLine("--> Connected To Message Bus");
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not Connect To The Message Bus: {e.Message}");
        }
    }

    private ConnectionFactory CreateFactory()
    {
        return new ConnectionFactory
        {
            HostName = _configuration["RabbitMQHost"], 
            Port = Convert.ToInt32(_configuration["RabbitMQPort"])
        };
    }
    
    public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
    {
        var message = JsonSerializer.Serialize(platformPublishDto);

        if (_connection.IsOpen)
        {
            Console.WriteLine("--> RabbitMQ Connection Open. Sending Message...");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("--> RabbitMQ Connection Closed. Not Sending Any Messages.");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        
        _channel.BasicPublish(
            exchange: "trigger",
            routingKey: "",
            basicProperties: null,
            body: body);
        
        Console.WriteLine($"--> Message Sent: {message}");
    }

    public void Dispose()
    {
        Console.WriteLine("--> Message Bus Disposed");
        
        if (!_channel.IsOpen) return;
        _channel.Close();
        _connection.Close();
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs args)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown");
    }
}