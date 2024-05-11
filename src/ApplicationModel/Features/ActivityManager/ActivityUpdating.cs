namespace Pomodorium.Features.ActivityManager;

public record ActivityUpdatingRequest : Request<ActivityUpdatingResponse>
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public DateTime? StartDateTime { get; init; }

    public DateTime? StopDateTime { get; init; }

    public string? Description { get; init; }

    public long Version { get; init; }
}

public record ActivityUpdatingResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
