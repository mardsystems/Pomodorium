namespace Pomodorium.Modules.Pomos;

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
