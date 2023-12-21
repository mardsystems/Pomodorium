using Newtonsoft.Json;
using Pomodorium.Enums;

namespace Pomodorium.Features.FlowTimer;

public record FlowtimeQueryRequest : Request<FlowtimeQueryResponse>
{
    public int? PageSize { get; init; }

    public int? PageIndex { get; init; }

    public FlowtimeQueryRequest(int? pageSize = null, int? pageIndex = null)
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
    }

    private FlowtimeQueryRequest() { }
}

public record FlowtimeQueryResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required IEnumerable<FlowtimeQueryItem> FlowtimeQueryItems { get; init; }
}

public class FlowtimeQueryItem
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public Guid TaskId { get; set; }

    public string? TaskDescription { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public bool? Interrupted { get; set; }

    public TimeSpan? Worktime { get; set; }

    public TimeSpan? Breaktime { get; set; }

    public TimeSpan? ExpectedDuration { get; set; }

    public FlowtimeStateEnum? State { get; set; }

    public long Version { get; set; }
}
