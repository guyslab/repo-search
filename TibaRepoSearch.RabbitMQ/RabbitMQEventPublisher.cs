using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TibaRepoSearch;

public class RabbitMQEventPublisher<TEvent> : IEventPublisher<TEvent>, IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _queueName;

    public RabbitMQEventPublisher(string hostName, string queueName)
    {
        _queueName = queueName;
        var factory = new ConnectionFactory { HostName = hostName };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null).GetAwaiter().GetResult();
    }

    public async Task PublishAsync(TEvent payload)
    {
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