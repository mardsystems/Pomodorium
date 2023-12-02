namespace Pomodorium.Features.TaskSynchronizer;

public class SyncTasksFromTrelloRequest : Request<SyncTasksFromTrelloResponse>
{
    
}

public class SyncTasksFromTrelloResponse : Response
{
    public SyncTasksFromTrelloResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public SyncTasksFromTrelloResponse() { }
}
