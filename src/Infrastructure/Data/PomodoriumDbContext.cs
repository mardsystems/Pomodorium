using Pomodorium.Modules.Pomos;
using System.Collections.ObjectModel;

namespace Pomodorium.Data;

public class PomodoriumDbContext
{
    public ObservableCollection<PomodoroDetails> PomodoroDetails { get; set; } = new ObservableCollection<PomodoroDetails>();
    
    public ObservableCollection<PomodoroQueryItem> PomodoroQueryItems { get; set; } = new ObservableCollection<PomodoroQueryItem>();
}
