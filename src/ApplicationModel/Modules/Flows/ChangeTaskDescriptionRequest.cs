namespace Pomodorium.Modules.Flows;

public class ChangeTaskDescriptionRequest : Request<ChangeTaskDescriptionResponse>
{
    public Guid TaskId { get; set; }

    public string TaskDescription { get; set; }

    public long TaskVersion { get; set; }
}
