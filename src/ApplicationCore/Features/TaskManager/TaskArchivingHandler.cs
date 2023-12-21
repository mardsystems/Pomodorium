namespace Pomodorium.Features.TaskManager;

public class TaskArchivingHandler : IRequestHandler<TaskArchivingRequest, TaskArchivingResponse>
{
    private readonly Repository _repository;

    public TaskArchivingHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<TaskArchivingResponse> Handle(TaskArchivingRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId) ?? throw new EntityNotFoundException();

        task.Archive();

        await _repository.Save(task, request.TaskVersion);

        var response = new TaskArchivingResponse(request.GetCorrelationId())
        {
            TaskVersion = task.Version
        };

        return response;
    }
}
