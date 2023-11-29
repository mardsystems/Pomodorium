namespace Pomodorium.Features.FlowTimer;

public class ArchiveFlowtimeRequest : Request<ArchiveFlowtimeResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}

public class ArchiveFlowtimeResponse : Response
{
    public ArchiveFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public ArchiveFlowtimeResponse() { }
}
