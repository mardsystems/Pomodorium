using Newtonsoft.Json;
using Pomodorium.Enums;

namespace Pomodorium.Features.PomodoroTimer;

public class GetPomodoroRequest : Request<GetPomodoroResponse>
{
    public Guid Id { get; set; }
}

public class GetPomodoroResponse : Response
{
    public GetPomodoroResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public PomodoroDetails PomodoroDetails { get; set; }

    public GetPomodoroResponse()
    {

    }
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
