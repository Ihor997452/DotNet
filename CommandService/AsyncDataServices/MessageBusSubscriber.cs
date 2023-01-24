using System.Text;
using DotNet.CommandService.EvenProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotNet.CommandService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;
        
        var factory = CreateFactory();
        
        _connection = factory.CreateConnection(); 
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout); 
        _queueName = _channel.QueueDeclare().QueueName; 
        _channel.QueueBind(
            queue: _queueName,
            exchange:"trigger",
            routingKey:"");
        _connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;
        Console.WriteLine("--> Connected To Message Bus");
    }

    private ConnectionFactory CreateFactory()
    {
        return new ConnectionFactory
        {
            HostName = _configuration["RabbitMQHost"], 
            Port = Convert.ToInt32(_configuration["RabbitMQPort"])
        };
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (moduleHandle, ea) =>
        {
            Console.WriteLine("--> Event Received");

            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
            
            _eventProcessor.ProcessEvent(message);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        
        return Task.CompletedTask;
    }

    private void RabbitMq_ConnectionShutdown(object? sender, ShutdownEventArgs shutdownEventArgs)
    {
        Console.WriteLine("--> Connection Shutdown");
    }

    public override void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
        
        base.Dispose();
    }
}