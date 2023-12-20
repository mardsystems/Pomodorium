using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class CreateFlowtimeFromTaskHandler : IRequestHandler<CreateFlowtimeFromTaskRequest, CreateFlowtimeFromTaskResponse>
{
    private readonly Repository _repository;

    public CreateFlowtimeFromTaskHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<CreateFlowtimeFromTaskResponse> Handle(CreateFlowtimeFromTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId);

        if (task.Description != request.TaskDescription)
        {
            task.ChangeDescription(request.TaskDescription);

            await _repository.Save(task, request.TaskVersion);

            task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId);
        }

        var flowtime = new Flowtime(task);

        await _repository.Save(flowtime, -1);

        var response = new CreateFlowtimeFromTaskResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
