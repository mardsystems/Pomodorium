namespace Pomodorium.Features.FlowTimer;

public record FlowtimeArchivingRequest : Request<FlowtimeArchivingResponse>
{
    public required Guid FlowtimeId { get; init; }

    public long FlowtimeVersion { get; init; }
}

public record FlowtimeArchivingResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
