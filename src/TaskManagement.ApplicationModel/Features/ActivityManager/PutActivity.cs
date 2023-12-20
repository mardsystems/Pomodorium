namespace Pomodorium.Features.ActivityManager;

public record PutActivityRequest : Request<PutActivityResponse>
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public DateTime? StartDateTime { get; init; }

    public DateTime? StopDateTime { get; init; }

    public string? Description { get; init; }

    public long Version { get; init; }
}

public record PutActivityResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
