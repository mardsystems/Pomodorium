using System.DomainModel.Storage;

namespace Pomodorium.Events;

public class GetEventsResponse : Response
{
    public GetEventsResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<EventRecord> EventRecords { get; set; }

    public GetEventsResponse()
    {

    }
}
