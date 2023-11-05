﻿namespace Pomodorium.Modules.Pomos;

public class PomodoroDetails
{
    public Guid Id { get; set; }

    public string? Task { get; set; }

    public TimeSpan Timer { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public PomodoroState State { get; set; }

    public long Version { get; set; }
}
