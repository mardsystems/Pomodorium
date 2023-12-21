namespace Pomodorium.Features.TaskManager;

public class TaskManagerRequestHandler :
    IRequestHandler<TaskQueryRequest, TaskQueryResponse>,
    IRequestHandler<TaskDetailsRequest, TaskDetailsResponse>,
    IRequestHandler<TaskRegistrationRequest, TaskRegistrationResponse>,
    IRequestHandler<TaskDescriptionChangeRequest, TaskDescriptionChangeResponse>,
    IRequestHandler<TaskArchiveRequest, TaskArchiveResponse>
{
    private readonly TaskManagerClient _client;

    public TaskManagerRequestHandler(TaskManagerClient client)
    {
        _client = client;
    }

    public async Task<TaskQueryResponse> Handle(TaskQueryRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetTasksAsync(request);

        return response;
    }

    public async Task<TaskDetailsResponse> Handle(TaskDetailsRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetTaskAsync(request, cancellationToken);

        return response;
    }

    public async Task<TaskRegistrationResponse> Handle(TaskRegistrationRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.CreateTaskAsync(request, cancellationToken);

        return response;
    }

    public async Task<TaskDescriptionChangeResponse> Handle(TaskDescriptionChangeRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.ChangeTaskDescriptionAsync(request, cancellationToken);

        return response;
    }

    public async Task<TaskArchiveResponse> Handle(TaskArchiveRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.ArchiveTaskAsync(request, cancellationToken);

        return response;
    }
}
