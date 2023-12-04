namespace Pomodorium.Features.TaskManager;

public class CreateTaskRequest : Request<CreateTaskResponse>
{
    public Guid? Id { get; set; }

    public string? Description { get; set; }

    public long? Version { get; set; }
}

public class CreateTaskResponse : Response
{
    public Guid TaskId { get; set; }

    public CreateTaskResponse(Guid correlationId, Guid taskId)
        : base(correlationId)
    {
        TaskId = taskId;
    }

    public CreateTaskResponse() { }
}
