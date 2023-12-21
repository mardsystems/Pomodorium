namespace Pomodorium.Features.ActivityManager;

public record ActivityDeletionRequest : Request<ActivityDeletionResponse>
{
    public Guid Id { get; init; }

    public long Version { get; init; }
}

public record ActivityDeletionResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
