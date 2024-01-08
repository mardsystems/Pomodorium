namespace TaskManagement.Features.TaskSynchronizer;

public record TaskSyncFromTfsRequest : Request<TaskSyncFromTfsResponse>
{
    
}

public record TaskSyncFromTfsResponse(Guid CorrelationId) : Response(CorrelationId)
{

}
