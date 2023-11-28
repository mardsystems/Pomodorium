namespace Pomodorium.Features.FlowTimer;

public class CreateFlowtimeRequest : Request<CreateFlowtimeResponse>
{
    public Guid? TaskId { get; set; }

    public string? TaskDescription { get; set; }

    public long? TaskVersion { get; set; }
}

public class CreateFlowtimeResponse : Response
{
    public CreateFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public CreateFlowtimeResponse() { }
}
