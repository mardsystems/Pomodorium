using MediatR;

namespace System.DomainModel.Storage;

public class EventAppended : INotification
{
    public EventRecord Record { get; set; }
}
