namespace Pomodorium.Features.TaskManager;

public record CreateTaskRequest : Request<CreateTaskResponse>
{
    public required string Description { get; init; }
}

public record CreateTaskResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required Guid TaskId { get; init; }

    public required long TaskVersion { get; init; }
}
