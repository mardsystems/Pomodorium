namespace Pomodorium.Features.TaskManager;

public class SyncTasksWithTFSRequest : Request<SyncTasksWithTFSResponse>
{
    public string ProjectName { get; set; }
}

public class SyncTasksWithTFSResponse : Response
{
    public SyncTasksWithTFSResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public SyncTasksWithTFSResponse() { }
}
