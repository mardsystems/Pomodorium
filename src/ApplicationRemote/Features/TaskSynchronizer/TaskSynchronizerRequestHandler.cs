namespace Pomodorium.Features.TaskSynchronizer;

public class TaskSynchronizerRequestHandler :
    IRequestHandler<SyncTasksFromTfsRequest, SyncTasksFromTfsResponse>,
    IRequestHandler<SyncTasksFromTrelloRequest, SyncTasksFromTrelloResponse>
{
    private readonly TaskSynchronizerClient _client;

    public TaskSynchronizerRequestHandler(TaskSynchronizerClient client)
    {
        _client = client;
    }

    public async Task<SyncTasksFromTfsResponse> Handle(SyncTasksFromTfsRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.SyncTasksFromTfsAsync(request);

        return response;
    }

    public async Task<SyncTasksFromTrelloResponse> Handle(SyncTasksFromTrelloRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.SyncTasksFromTrelloAsync(request, cancellationToken);

        return response;
    }
}
