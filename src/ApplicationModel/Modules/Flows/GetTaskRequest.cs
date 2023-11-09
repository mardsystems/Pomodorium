namespace Pomodorium.Modules.Flows;

public class GetTaskRequest : Request<GetTaskResponse>
{
    public Guid Id { get; set; }
}
