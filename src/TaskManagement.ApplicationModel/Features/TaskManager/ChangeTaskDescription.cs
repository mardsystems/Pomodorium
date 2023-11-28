namespace Pomodorium.Features.TaskManager;

public class ChangeTaskDescriptionRequest : Request<ChangeTaskDescriptionResponse>
{
    public Guid TaskId { get; set; }

    public string TaskDescription { get; set; }

    public long TaskVersion { get; set; }
}

public class ChangeTaskDescriptionResponse : Response
{
    public ChangeTaskDescriptionResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public ChangeTaskDescriptionResponse() { }
}
