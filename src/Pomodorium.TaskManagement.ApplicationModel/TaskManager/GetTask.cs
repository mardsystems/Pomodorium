namespace Pomodorium.TaskManagement.TaskManager;

public class GetTaskRequest : Request<GetTaskResponse>
{
    public Guid Id { get; set; }
}

public class GetTaskResponse : Response
{
    public GetTaskResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public TaskDetails TaskDetails { get; set; }

    public GetTaskResponse()
    {

    }
}
