namespace Pomodorium.Features.ActivityManager;

public record DeleteActivityRequest : Request<DeleteActivityResponse>
{
    public Guid Id { get; init; }

    public long Version { get; init; }
}

public record DeleteActivityResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
