using Newtonsoft.Json;
using Pomodorium.Enums;

namespace Pomodorium.Features.PomodoroTimer;

public record PomodoroQueryRequest : Request<PomodoroQueryResponse>
{
    public int? PageSize { get; init; }

    public int? PageIndex { get; init; }
}

public record PomodoroQueryResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required IEnumerable<PomodoroQueryItem> PomodoroQueryItems { get; init; }
}

public class PomodoroQueryItem
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    public string? Task { get; set; }

    public TimeSpan Timer { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public PomodoroStateEnum State { get; set; }

    public long Version { get; set; }
}
