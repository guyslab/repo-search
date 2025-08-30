using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class RabbitMQEventPublisher<TEvent> : IEventPublisher<TEvent>, IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _queueName;
    private readonly ILogger<RabbitMQEventPublisher<TEvent>> _logger;

    public RabbitMQEventPublisher(string hostName, string queueName, ILogger<RabbitMQEventPublisher<TEvent>> logger, string userName = "guest", string password = "guest")
    {
        _queueName = queueName;
        _connectionFactory = new ConnectionFactory { HostName = hostName, UserName = userName, Password = password };
        _logger = logger;
        _logger.LogTrace("[RabbitMQEventPublisher<{EventType}>..ctor] {hostName};{queueName} OK", typeof(TEvent).Name, hostName, queueName);
    }

    public async Task PublishAsync(TEvent payload)
    {
        try
        {
            _connection ??= await _connectionFactory.CreateConnectionAsync();
            if (_channel == null)
            {
                _channel = await _connection.CreateChannelAsync();
                await _channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            }
            var message = JsonSerializer.Serialize(payload);
            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: _queueName, body: body);
            _logger.LogTrace("[RabbitMQEventPublisher<{EventType}>.PublishAsync] {payload} OK", typeof(TEvent).Name, payload);
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[RabbitMQEventPublisher<{EventType}>.PublishAsync] {payload} {Message}", typeof(TEvent).Name, payload, ex.Message);
            throw;
        }
    }

    public void Dispose()
    {
        _channel?.CloseAsync().Wait();
        _connection?.CloseAsync().Wait();
    }
}