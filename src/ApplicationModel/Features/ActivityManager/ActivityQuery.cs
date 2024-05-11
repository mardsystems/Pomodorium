using Newtonsoft.Json;
using Pomodorium.Enums;

namespace Pomodorium.Features.ActivityManager;

public record ActivityQueryRequest : Request<ActivityQueryResponse>
{
    public int? PageSize { get; init; }

    public int? PageIndex { get; init; }
}

public record ActivityQueryResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required IEnumerable<ActivityQueryItem> ActivityQueryItems { get; init; }
}

public class ActivityQueryItem
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
