namespace Pomodorium.Features.TaskManager;

public class TaskDescriptionChangeHandler : IRequestHandler<TaskDescriptionChangeRequest, TaskDescriptionChangeResponse>
{
    private readonly Repository _repository;

    public TaskDescriptionChangeHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<TaskDescriptionChangeResponse> Handle(TaskDescriptionChangeRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId) ?? throw new EntityNotFoundException();

        task.ChangeDescription(request.Description);

        await _repository.Save(task, request.TaskVersion);

        var response = new TaskDescriptionChangeResponse(request.GetCorrelationId())
        {
            TaskVersion = task.Version
        };

        return response;
    }
}
