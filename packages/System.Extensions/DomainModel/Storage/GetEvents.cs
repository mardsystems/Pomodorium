using System.ApplicationModel;

namespace System.DomainModel.Storage;

public class GetEventsRequest : Request<GetEventsResponse>
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}
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
