using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace TibaRepoSearch;

public class MessageReceiver : IDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IRepositoryFavoritedEventHandler _handler;
    private readonly string _hostName;
    private readonly string _queueName;
    private readonly string _userName;
    private readonly string _password;

    public MessageReceiver(IRepositoryFavoritedEventHandler handler, string hostName, string queueName, string userName = "guest", string password = "guest")
    {
        _handler = handler;
        _hostName = hostName;
        _queueName = queueName;
        _userName = userName;
        _password = password;
    }

    public async Task StartListeningAsync()
    {
        var factory = new ConnectionFactory { HostName = _hostName, UserName = _userName, Password = _password };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var repositoryMessage = JsonSerializer.Deserialize<RepositoryFavoritedMessage>(message);
            
            if (repositoryMessage != null)
            {
                await _handler.Handle(repositoryMessage);
            }
        };

        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
    }

    public void Dispose()
    {
        if (_channel != null)
            _channel.CloseAsync().Wait();
        if (_connection != null)
            _connection.CloseAsync().Wait();
    }
}