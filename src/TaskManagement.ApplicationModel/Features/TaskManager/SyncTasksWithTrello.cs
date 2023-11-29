namespace Pomodorium.Features.TaskManager;

public class SyncTasksWithTrelloRequest : Request<SyncTasksWithTrelloResponse>
{
    public string BoardId { get; set; }
}

public class SyncTasksWithTrelloResponse : Response
{
    public SyncTasksWithTrelloResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public SyncTasksWithTrelloResponse() { }
}
