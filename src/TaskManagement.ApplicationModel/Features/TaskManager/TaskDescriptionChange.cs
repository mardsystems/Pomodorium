namespace Pomodorium.Features.TaskManager;

public record TaskDescriptionChangeRequest : Request<TaskDescriptionChangeResponse>
{
    public required Guid TaskId { get; init; }

    public required string Description { get; init; }

    public long TaskVersion { get; init; }
}

public record TaskDescriptionChangeResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required long TaskVersion { get; init; }
}
