namespace Pomodorium.TaskManagement.TaskManager;

public class GetTasksRequest : Request<GetTasksResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}

public class GetTasksResponse : Response
{
    public GetTasksResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<TaskQueryItem> TaskQueryItems { get; set; }

    public GetTasksResponse() { }
}
