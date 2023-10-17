using MediatR;
using Pomodorium.Data;

namespace Pomodorium.Modules.Pomodori;

public class PomodoroEventHandler :
    INotificationHandler<PomodoroCreated>
{
    private readonly PomodoriumDbContext _db;

    public PomodoroEventHandler(PomodoriumDbContext db)
    {
        _db = db;
    }

    public async Task Handle(PomodoroCreated request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem
        {
            Id = request.Id.ToString(),
            StartDateTime = request.StartDateTime,
            Description = request.Description
        };

        _db.PomodoroQueryItems.Add(pomodoroQueryItem);

        await Task.CompletedTask;
    }
}
