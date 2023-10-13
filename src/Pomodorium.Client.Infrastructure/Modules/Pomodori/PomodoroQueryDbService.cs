using Pomodorium.Data;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Pomodorium.Modules.Pomodori;

public class PomodoroQueryDbService
{
    private readonly PomodoriumDbContext _db;

    public PomodoroQueryDbService(PomodoriumDbContext db)
    {
        _db = db;
    }

    public async Task<ObservableCollection<PomodoroQueryItem>> QueryPomodori()
    {
        var items = _db.PomodoroQueryItems;

        return await Task.FromResult(items);
    }
}
