﻿namespace Pomodorium;

public class GetPomodoroResponse : Response
{
    public GetPomodoroResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public PomodoroDetails PomodoroDetails { get; set; }
}
