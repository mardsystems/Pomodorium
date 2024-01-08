namespace TaskManagement.Features.TaskSynchronizer;

public record TaskSyncFromTrelloRequest : Request<TaskSyncFromTrelloResponse>
{
    
}

public record TaskSyncFromTrelloResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
