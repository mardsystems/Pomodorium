namespace PomodoroTechnique.Features.PomodoroTimer;

public record PomodoroCreationRequest : Request<PomodoroCreationResponse>
{
    public required string Task { get; init; }

    public TimeSpan Timer { get; init; }
}

public record PomodoroCreationResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
