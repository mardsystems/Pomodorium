namespace Pomodorium.Modules.Flows;

public class CreateTaskRequest : Request<CreateTaskResponse>
{
    public Guid? Id { get; set; }

    public string? Description { get; set; }

    public long? Version { get; set; }
}
