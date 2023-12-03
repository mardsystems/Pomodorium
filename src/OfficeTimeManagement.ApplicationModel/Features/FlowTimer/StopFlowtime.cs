namespace Pomodorium.Features.FlowTimer;

public class StopFlowtimeRequest : Request<StopFlowtimeResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}

public class StopFlowtimeResponse : Response
{
    public StopFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public StopFlowtimeResponse() { }
}
