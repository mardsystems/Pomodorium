using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.DomainModel.Storage;
using System.Text;
using System.Text.Json;

namespace Pomodorium.Bus;

public class RabbitMQConsumer: IDisposable
{
    private readonly IConnection _connection;

    private readonly IModel _channel;

    private readonly IMediator _mediator;

    public RabbitMQConsumer(IConnection connection, IMediator mediator)
    {
        _connection = connection;

        _channel = _connection.CreateModel();

        _mediator = mediator;
    }

    public void Start()
    {
        _channel.QueueDeclare(
            queue: "events",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var basicConsumer = new EventingBasicConsumer(_channel);

        basicConsumer.Received += Consumer_Received;

        _channel.BasicConsume(
            queue: "events",
            autoAck: false,
            consumer: basicConsumer
        );
    }

    private async void Consumer_Received(object sender, BasicDeliverEventArgs args)
    {
        var body = args.Body.ToArray();

        var content = Encoding.UTF8.GetString(body);

        var notification = JsonSerializer.Deserialize<EventAppended>(content);

        await _mediator.Publish(notification);

        _channel.BasicAck(args.DeliveryTag, false);
    }

    public void Dispose()
    {
        _channel.Dispose();
    }
}
