namespace Pomodorium.Features.ActivityManager;

public record PostActivityRequest : Request<PostActivityResponse>
{
    public required string Name { get; init; }

    public DateTime? StartDateTime { get; init; }

    public DateTime? StopDateTime { get; init; }

    public string? Description { get; init; }
}

public record PostActivityResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
