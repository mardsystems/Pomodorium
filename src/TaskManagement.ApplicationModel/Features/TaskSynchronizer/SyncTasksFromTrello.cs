namespace Pomodorium.Features.TaskSynchronizer;

public class SyncTasksFromTrelloRequest : Request<SyncTasksFromTrelloResponse>
{
    public string BoardId { get; set; }
}

public class SyncTasksFromTrelloResponse : Response
{
    public SyncTasksFromTrelloResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public SyncTasksFromTrelloResponse() { }
}
