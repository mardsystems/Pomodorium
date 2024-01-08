namespace RoutineTracking.Features.RoutineTracker;

public record StartRoutineRequest : Request<StartRoutineResponse>
{
    public Guid Id { get; init; }

    public DateTime StartDateTime { get; init; }

    public long Version { get; init; }
}

public record StartRoutineResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
