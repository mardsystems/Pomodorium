namespace Pomodorium.Features.TaskManager;

public record TaskRegistrationRequest : Request<TaskRegistrationResponse>
{
    public required string Description { get; init; }
}

public record TaskRegistrationResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required Guid TaskId { get; init; }

    public required long TaskVersion { get; init; }
}
