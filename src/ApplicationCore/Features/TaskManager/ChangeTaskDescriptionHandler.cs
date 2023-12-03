namespace Pomodorium.Features.TaskManager;

public class ChangeTaskDescriptionHandler : IRequestHandler<ChangeTaskDescriptionRequest, ChangeTaskDescriptionResponse>
{
    private readonly Repository _repository;

    public ChangeTaskDescriptionHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<ChangeTaskDescriptionResponse> Handle(ChangeTaskDescriptionRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId);

        if (task == null)
        {
            throw new EntityNotFoundException();
        }

        task.ChangeDescription(request.TaskDescription);

        await _repository.Save(task, request.TaskVersion);

        var response = new ChangeTaskDescriptionResponse(request.GetCorrelationId()) { };

        return response;
    }
}
