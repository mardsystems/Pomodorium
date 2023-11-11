namespace Pomodorium.Modules.Flows;

public class GetTasksResponse : Response
{
    public GetTasksResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<TaskQueryItem> TaskQueryItems { get; set; }

    public GetTasksResponse() { }
}
