namespace Pomodorium.Features.FlowTimer;

public record ArchiveFlowtimeRequest : Request<ArchiveFlowtimeResponse>
{
    public Guid FlowtimeId { get; init; }

    public long FlowtimeVersion { get; init; }
}

public record ArchiveFlowtimeResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
