using Newtonsoft.Json;
using Pomodorium.Enums;

namespace Pomodorium.Features.PomodoroTimer;

public record GetPomodoroRequest : Request<GetPomodoroResponse>
{
    public Guid Id { get; init; }
}

public record GetPomodoroResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required PomodoroDetails PomodoroDetails { get; init; }
}

public class PomodoroDetails
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
