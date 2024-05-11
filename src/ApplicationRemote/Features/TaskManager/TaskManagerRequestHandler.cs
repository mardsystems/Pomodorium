namespace Pomodorium.Features.TaskManager;

public class TaskManagerRequestHandler :
    IRequestHandler<TaskQueryRequest, TaskQueryResponse>,
    IRequestHandler<TaskDetailsRequest, TaskDetailsResponse>,
    IRequestHandler<TaskRegistrationRequest, TaskRegistrationResponse>,
    IRequestHandler<TaskDescriptionChangeRequest, TaskDescriptionChangeResponse>,
    IRequestHandler<TaskArchivingRequest, TaskArchivingResponse>
{
    private readonly TaskManagerClient _client;

    public TaskManagerRequestHandler(TaskManagerClient client)
    {
        _client = client;
    }

    public async Task<TaskQueryResponse> Handle(TaskQueryRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetTaskQueryAsync(request.PageSize, request.PageIndex, request.Description, request.ExternalReference, cancellationToken);

        return response;
    }

    public async Task<TaskDetailsResponse> Handle(TaskDetailsRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetTaskDetailsAsync(request.TaskId, cancellationToken);

        return response;
    }

    public async Task<TaskRegistrationResponse> Handle(TaskRegistrationRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.PostTaskRegistrationAsync(request, cancellationToken);

        return response;
    }

    public async Task<TaskDescriptionChangeResponse> Handle(TaskDescriptionChangeRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.PostTaskDescriptionChangeAsync(request, cancellationToken);

        return response;
    }

    public async Task<TaskArchivingResponse> Handle(TaskArchivingRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.PostTaskArchivingAsync(request, cancellationToken);

        return response;
    }
}
