﻿namespace Pomodorium.Features.ActivityManager;

public class PutActivityRequest : Request<PutActivityResponse>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public string Description { get; set; }

    public long Version { get; set; }
}

public class PutActivityResponse : Response
{
    public PutActivityResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public PutActivityResponse()
    {

    }
}
