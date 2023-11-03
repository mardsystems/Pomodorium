namespace Pomodorium.Modules.Flows;

public class GetFlowsResponse : Response
{
    public GetFlowsResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<FlowtimeQueryItem> FlowtimeQueryItems { get; set; }

    public GetFlowsResponse() { }
}
