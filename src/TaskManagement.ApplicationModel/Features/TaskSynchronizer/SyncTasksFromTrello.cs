namespace Pomodorium.Features.TaskSynchronizer;

public record SyncTasksFromTrelloRequest : Request<SyncTasksFromTrelloResponse>
{
    
}

public record SyncTasksFromTrelloResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
