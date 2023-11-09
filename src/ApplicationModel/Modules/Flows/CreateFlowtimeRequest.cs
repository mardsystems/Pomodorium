namespace Pomodorium.Modules.Flows;

public class CreateFlowtimeRequest : Request<CreateFlowtimeResponse>
{
    public Guid? TaskId { get; set; }

    public string? TaskDescription { get; set; }

    public long? TaskVersion { get; set; }
}
