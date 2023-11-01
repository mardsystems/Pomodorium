using MediatR;
using Pomodorium.Bus;
using System.DomainModel.Storage;

namespace Pomodorium.Handlers;

public class RabbitMQEventHandler : INotificationHandler<EventAppended>
{
    private readonly RabbitMQPublisher _bus;

    public RabbitMQEventHandler(RabbitMQPublisher bus)
    {
        _bus = bus;
    }

    public async Task Handle(EventAppended notification, CancellationToken cancellationToken)
    {
        await _bus.Publish(notification);
    }
}
