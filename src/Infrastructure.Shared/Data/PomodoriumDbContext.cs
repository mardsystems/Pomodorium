﻿using Pomodorium.Features.PomodoroTimer;
using System.Collections.ObjectModel;

namespace Pomodorium.Data;

public class PomodoriumDbContext
{
    public ObservableCollection<PomodoroDetails> PomodoroDetails { get; set; } = new ObservableCollection<PomodoroDetails>();
    
    public ObservableCollection<PomodoroQueryItem> PomodoroQueryItems { get; set; } = new ObservableCollection<PomodoroQueryItem>();
}