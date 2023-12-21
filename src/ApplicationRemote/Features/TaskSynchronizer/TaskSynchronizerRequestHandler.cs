namespace Pomodorium.Features.TaskSynchronizer;

public class TaskSynchronizerRequestHandler :
    IRequestHandler<TaskSyncFromTfsRequest, TaskSyncFromTfsResponse>,
    IRequestHandler<TaskSyncFromTrelloRequest, TaskSyncFromTrelloResponse>
{
    private readonly TaskSynchronizerClient _client;

    public TaskSynchronizerRequestHandler(TaskSynchronizerClient client)
    {
        _client = client;
    }

    public async Task<TaskSyncFromTfsResponse> Handle(TaskSyncFromTfsRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.PostTaskSyncFromTfsAsync(request, cancellationToken);

        return response;
    }

    public async Task<TaskSyncFromTrelloResponse> Handle(TaskSyncFromTrelloRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.PostTaskSyncFromTrelloAsync(request, cancellationToken);

        return response;
    }
}
