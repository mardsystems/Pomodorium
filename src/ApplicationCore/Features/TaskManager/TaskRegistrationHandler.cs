namespace Pomodorium.Features.TaskManager;

public class TaskRegistrationHandler : IRequestHandler<TaskRegistrationRequest, TaskRegistrationResponse>
{
    private readonly Repository _repository;

    public TaskRegistrationHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<TaskRegistrationResponse> Handle(TaskRegistrationRequest request, CancellationToken cancellationToken)
    {
        var task = new Models.TaskManagement.Tasks.Task(request.Description);

        await _repository.Save(task, -1);

        var response = new TaskRegistrationResponse(request.GetCorrelationId())
        {
            TaskId = task.Id,
            TaskVersion = task.Version,
        };

        return response;
    }
}
