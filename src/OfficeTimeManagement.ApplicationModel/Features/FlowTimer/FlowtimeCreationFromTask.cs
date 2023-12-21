namespace Pomodorium.Features.FlowTimer;

public record FlowtimeCreationFromTaskRequest : Request<FlowtimeCreationFromTaskResponse>
{
    public Guid TaskId { get; init; }

    public required string TaskDescription { get; init; }

    public long TaskVersion { get; init; }
}

public record FlowtimeCreationFromTaskResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long FlowtimeVersion { get; init; }
}
