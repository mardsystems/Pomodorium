namespace Pomodorium.Features.ActivityManager;

public class DeleteActivityRequest : Request<DeleteActivityResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}

public class DeleteActivityResponse : Response
{
    public DeleteActivityResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public DeleteActivityResponse()
    {

    }
}
