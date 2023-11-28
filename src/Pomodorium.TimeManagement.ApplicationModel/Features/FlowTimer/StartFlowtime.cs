namespace Pomodorium.Features.FlowTimer;

public class StartFlowtimeRequest : Request<StartFlowtimeResponse>
{
    public Guid Id { get; set; }

    public DateTime StartDateTime { get; set; }

    public long Version { get; set; }
}

public class StartFlowtimeResponse : Response
{
    public StartFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public StartFlowtimeResponse() { }
}
