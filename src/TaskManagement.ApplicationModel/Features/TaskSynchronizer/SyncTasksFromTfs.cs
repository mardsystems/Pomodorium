namespace Pomodorium.Features.TaskSynchronizer;

public class SyncTasksFromTfsRequest : Request<SyncTasksFromTfsResponse>
{
    
}

public class SyncTasksFromTfsResponse : Response
{
    public SyncTasksFromTfsResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public SyncTasksFromTfsResponse() { }
}
