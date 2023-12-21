using Newtonsoft.Json;
using Pomodorium.Enums;

namespace Pomodorium.Features.ActivityManager;

public record ActivityDetailsRequest : Request<ActivityDetailsResponse>
{
    public Guid Id { get; init; }
}

public record ActivityDetailsResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required ActivityDetails ActivityDetails { get; init; }
}

public class ActivityDetails
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public ActivityStateEnum State { get; set; }

    public TimeSpan? Duration { get; set; }

    public string? Description { get; set; }

    public long Version { get; set; }
}
