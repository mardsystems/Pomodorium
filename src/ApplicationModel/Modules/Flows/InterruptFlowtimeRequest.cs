namespace Pomodorium.Modules.Flows;

public class InterruptFlowtimeRequest : Request<InterruptFlowtimeResponse>
{
    public Guid Id { get; set; }

    public DateTime InterruptDateTime { get; set; }

    public long Version { get; set; }
}
