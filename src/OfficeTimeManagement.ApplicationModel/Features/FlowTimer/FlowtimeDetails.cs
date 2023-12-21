using Newtonsoft.Json;
using Pomodorium.Enums;
using System.ComponentModel.DataAnnotations;

namespace Pomodorium.Features.FlowTimer;

public record FlowtimeDetailsRequest(Guid FlowtimeId) : Request<FlowtimeDetailsResponse>
{

}

public record FlowtimeDetailsResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required FlowtimeDetails FlowtimeDetails { get; init; }
}

public class FlowtimeDetails
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public Guid TaskId { get; set; }

    public string? TaskDescription { get; set; }

    public long TaskVersion { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public bool? Interrupted { get; set; }

    [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}")]
    public TimeSpan? Worktime { get; set; }

    public TimeSpan? Breaktime { get; set; }

    public TimeSpan? ExpectedDuration { get; set; }

    public FlowtimeStateEnum? State { get; set; }

    public long Version { get; set; }
}
