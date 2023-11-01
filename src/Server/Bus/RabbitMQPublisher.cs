using RabbitMQ.Client;
using System.DomainModel.Storage;
using System.Text;
using System.Text.Json;

namespace Pomodorium.Bus;

public class RabbitMQPublisher : IDisposable
{
    private readonly IConnection _connection;

    private readonly IModel _channel;

    public RabbitMQPublisher(IConnection connection)
    {
        _connection = connection;

        _channel = _connection.CreateModel();
    }

    public void Config()
    {
        _channel.QueueDeclare(
            queue: "events",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    public async Task Publish(EventAppended notification)
    {
        var content = JsonSerializer.Serialize(notification);

        var body = Encoding.UTF8.GetBytes(content);

        _channel.BasicPublish(
            exchange: "",
            routingKey: "events",
            basicProperties: null,
            body: body
        );

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel.Dispose();
    }
}
