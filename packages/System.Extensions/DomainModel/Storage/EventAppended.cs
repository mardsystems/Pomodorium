using MediatR;

namespace System.DomainModel.Storage;

public class EventAppended : INotification
{
    public required EventRecord Record { get; set; }
}
