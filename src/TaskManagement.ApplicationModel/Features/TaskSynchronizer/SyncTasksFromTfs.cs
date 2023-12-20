namespace Pomodorium.Features.TaskSynchronizer;

public record SyncTasksFromTfsRequest : Request<SyncTasksFromTfsResponse>
{
    
}

public record SyncTasksFromTfsResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
