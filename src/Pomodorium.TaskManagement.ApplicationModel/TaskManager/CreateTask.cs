namespace Pomodorium.TaskManagement.TaskManager;

public class CreateTaskRequest : Request<CreateTaskResponse>
{
    public Guid? Id { get; set; }

    public string? Description { get; set; }

    public long? Version { get; set; }
}

public class CreateTaskResponse : Response
{
    public CreateTaskResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public CreateTaskResponse() { }
}
