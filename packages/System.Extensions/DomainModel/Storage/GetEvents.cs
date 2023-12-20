using System.ApplicationModel;

namespace System.DomainModel.Storage;

public record GetEventsRequest : Request<GetEventsResponse>
{
    public int? PageSize { get; init; }

    public int? PageIndex { get; init; }
}

public record GetEventsResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required IEnumerable<EventRecord> EventRecords { get; init; }
}
