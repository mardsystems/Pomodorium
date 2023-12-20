namespace Pomodorium.Features.TaskManager;

public record ChangeTaskDescriptionRequest : Request<ChangeTaskDescriptionResponse>
{
    public Guid TaskId { get; init; }

    public required string Description { get; init; }

    public long TaskVersion { get; init; }
}

public record ChangeTaskDescriptionResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long TaskVersion { get; init; }
}
