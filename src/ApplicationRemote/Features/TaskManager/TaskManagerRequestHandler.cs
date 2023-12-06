namespace Pomodorium.Features.TaskManager;

public class TaskManagerRequestHandler :
    IRequestHandler<GetTasksRequest, GetTasksResponse>,
    IRequestHandler<GetTaskRequest, GetTaskResponse>,
    IRequestHandler<CreateTaskRequest, CreateTaskResponse>,
    IRequestHandler<ChangeTaskDescriptionRequest, ChangeTaskDescriptionResponse>,
    IRequestHandler<ArchiveTaskRequest, ArchiveTaskResponse>
{
    private readonly TaskManagerClient _client;

    public TaskManagerRequestHandler(TaskManagerClient client)
    {
        _client = client;
    }

    public async Task<GetTasksResponse> Handle(GetTasksRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetTasksAsync(request);

        return response;
    }

    public async Task<GetTaskResponse> Handle(GetTaskRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetTaskAsync(request, cancellationToken);

        return response;
    }

    public async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.CreateTaskAsync(request, cancellationToken);

        return response;
    }

    public async Task<ChangeTaskDescriptionResponse> Handle(ChangeTaskDescriptionRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.ChangeTaskDescriptionAsync(request, cancellationToken);

        return response;
    }

    public async Task<ArchiveTaskResponse> Handle(ArchiveTaskRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.ArchiveTaskAsync(request, cancellationToken);

        return response;
    }
}
