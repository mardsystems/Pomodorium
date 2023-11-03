namespace Pomodorium.Modules.Flows;

public class GetFlowsRequest : Request<GetFlowsResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}
