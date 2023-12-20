namespace Pomodorium.Features.TaskManager;

public class CreateTaskHandler : IRequestHandler<CreateTaskRequest, CreateTaskResponse>
{
    private readonly Repository _repository;

    public CreateTaskHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = new Models.TaskManagement.Tasks.Task(request.Description);

        await _repository.Save(task, -1);

        var response = new CreateTaskResponse(request.GetCorrelationId())
        {
            TaskId = task.Id,
            TaskVersion = task.Version,
        };

        return response;
    }
}
