using Pomodorium.Modules.Pomodori;

namespace Pomodorium;

public class GetPomodoriResponse : Response
{
    public GetPomodoriResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public PomodoroQueryItem[] PomodoroQueryItems { get; set; }

    public GetPomodoriResponse()
    {
        
    }
}
