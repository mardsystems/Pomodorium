namespace PomodoroTechnique.Features.PomodoroTimer;

public record PomodoroCheckingRequest : Request<PomodoroCheckingResponse>
{
    public Guid Id { get; init; }

    public long Version { get; init; }
}

public record PomodoroCheckingResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
