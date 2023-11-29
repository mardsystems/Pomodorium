﻿using Pomodorium.PomodoroTechnique.Model;

namespace Pomodorium.Features.PomodoroTimer;

public class GetPomosRequest : Request<GetPomosResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}

public class GetPomosResponse : Response
{
    public GetPomosResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<PomodoroQueryItem> PomodoroQueryItems { get; set; }

    public GetPomosResponse()
    {

    }
}

public class PomodoroQueryItem
{
    public Guid Id { get; set; }

    public string? Task { get; set; }

    public TimeSpan Timer { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public PomodoroState State { get; set; }

    public long Version { get; set; }
}