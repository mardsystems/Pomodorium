namespace Pomodorium.Features.ActivityManager;

public class PostActivityRequest : Request<PostActivityResponse>
{
    public string Name { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public string Description { get; set; }
}

public class PostActivityResponse : Response
{
    public PostActivityResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public PostActivityResponse()
    {

    }
}
