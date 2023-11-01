using Pomodorium.Data;
using System.Collections.ObjectModel;

namespace Pomodorium.Modules.Timers;

public class PomodoroQueryDbService
{
    private readonly PomodoriumDbContext _db;

    public PomodoroQueryDbService(PomodoriumDbContext db)
    {
        _db = db;
    }

    public async Task<ObservableCollection<PomodoroQueryItem>> QueryPomodoroItems()
    {
        var items = _db.PomodoroQueryItems;

        return await Task.FromResult(items);
    }
}
