namespace Pomodorium.Features.RoutineTracker;

public record GetRoutinesRequest : Request<GetRoutinesResponse>
{
    public int? PageSize { get; init; }

    public int? PageIndex { get; init; }
}

public record GetRoutinesResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required IEnumerable<RoutineQueryItem> RoutineQueryItems { get; init; }
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
