namespace Pomodorium.Modules.Flows;

public class StopFlowtimeRequest : Request<StopFlowtimeResponse>
{
    public Guid Id { get; set; }

    public DateTime StopDateTime { get; set; }

    public long Version { get; set; }
}
