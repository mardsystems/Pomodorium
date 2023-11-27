namespace Pomodorium.TimeManagement.PomodoroTimer;

public class GetPomosRequest : Request<GetPomosResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}

public class GetPomosResponse : Response
{
    public GetPomosResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<PomodoroQueryItem> PomodoroQueryItems { get; set; }

    public GetPomosResponse()
    {

    }
}
