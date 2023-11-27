namespace Pomodorium.TimeManagement.FlowTimer;

public class GetFlowsRequest : Request<GetFlowsResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}

public class GetFlowsResponse : Response
{
    public GetFlowsResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<FlowtimeQueryItem> FlowtimeQueryItems { get; set; }

    public GetFlowsResponse() { }
}
