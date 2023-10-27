using Pomodorium.Modules.Pomodori;
using System.Collections.ObjectModel;

namespace Pomodorium.Data;

public class PomodoriumDbContext
{
    public ObservableCollection<PomodoroQueryItem> PomodoroQueryItems { get; set; } = new ObservableCollection<PomodoroQueryItem>();
}
