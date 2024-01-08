namespace TaskManagement.Features.ActivityManager;

public record ActivityCreationRequest : Request<ActivityCreationResponse>
{
    public required string Name { get; init; }

    public DateTime? StartDateTime { get; init; }

    public DateTime? StopDateTime { get; init; }

    public string? Description { get; init; }
}

public record ActivityCreationResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
