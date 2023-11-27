﻿using Pomodorium.TaskManagement.Activities;

namespace Pomodorium.TaskManagement.ActivityManager;

public class ActivityDetails
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public ActivityState State { get; set; }

    public TimeSpan? Duration { get; set; }

    public string Description { get; set; }

    public long Version { get; set; }
}