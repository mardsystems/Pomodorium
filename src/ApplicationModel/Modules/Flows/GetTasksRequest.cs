namespace Pomodorium.Modules.Flows;

public class GetTasksRequest : Request<GetTasksResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}
