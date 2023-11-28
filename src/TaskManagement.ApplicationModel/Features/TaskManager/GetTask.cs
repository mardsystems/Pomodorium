namespace Pomodorium.Features.TaskManager;

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

public class TaskDetails
{
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? Description { get; set; }

    public long Version { get; set; }
}
