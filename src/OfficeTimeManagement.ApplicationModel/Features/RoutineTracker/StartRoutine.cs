namespace Pomodorium.Features.RoutineTracker;

public class StartRoutineRequest : Request<StartRoutineResponse>
{
    public Guid Id { get; set; }

    public DateTime StartDateTime { get; set; }

    public long Version { get; set; }
}

public class StartRoutineResponse : Response
{
    public StartRoutineResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public StartRoutineResponse() { }
}
