using Newtonsoft.Json;
using Pomodorium.Enums;

namespace Pomodorium.Features.FlowTimer;

public class GetFlowsRequest : Request<GetFlowsResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}

public class GetFlowsResponse : Response
{
    public GetFlowsResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<FlowtimeQueryItem> FlowtimeQueryItems { get; set; }

    public GetFlowsResponse() { }
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
