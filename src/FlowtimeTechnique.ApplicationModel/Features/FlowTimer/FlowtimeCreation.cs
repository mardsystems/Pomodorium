namespace Pomodorium.Features.FlowTimer;

public record FlowtimeCreationRequest : Request<FlowtimeCreationResponse>
{
    public required string TaskDescription { get; init; }
}

public record FlowtimeCreationResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required Guid FlowtimeId { get; init; }

    public required long FlowtimeVersion { get; init; }
}
