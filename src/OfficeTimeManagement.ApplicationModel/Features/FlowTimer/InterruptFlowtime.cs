namespace Pomodorium.Features.FlowTimer;

public class InterruptFlowtimeRequest : Request<InterruptFlowtimeResponse>
{
    public Guid Id { get; set; }

    public DateTime InterruptDateTime { get; set; }

    public long Version { get; set; }
}

public class InterruptFlowtimeResponse : Response
{
    public InterruptFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public InterruptFlowtimeResponse() { }
}
