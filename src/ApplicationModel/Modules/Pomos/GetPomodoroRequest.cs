﻿namespace Pomodorium.Modules.Pomos;

public class GetPomodoroRequest : Request<GetPomodoroResponse>
{
    public Guid Id { get; set; }
}