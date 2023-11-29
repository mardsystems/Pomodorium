namespace Pomodorium.Features.RoutineTracker;

public class GetRoutinesRequest : Request<GetRoutinesResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}

public class GetRoutinesResponse : Response
{
    public GetRoutinesResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<RoutineQueryItem> RoutineQueryItems { get; set; }

    public GetRoutinesResponse() { }
}

public class RoutineQueryItem
{
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

    //public RoutineState? State { get; set; }

    public long Version { get; set; }
}
