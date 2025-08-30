using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TibaRepoSearch;

public class RabbitMQEventPublisher<TEvent> : IEventPublisher<TEvent>, IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _queueName;

    public RabbitMQEventPublisher(string hostName, string queueName)
    {
        _queueName = queueName;
        _connectionFactory = new ConnectionFactory { HostName = hostName };
    }

    public async Task PublishAsync(TEvent payload)
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
    }

    public void Dispose()
    {
        _channel?.CloseAsync().Wait();
        _connection?.CloseAsync().Wait();
    }
}