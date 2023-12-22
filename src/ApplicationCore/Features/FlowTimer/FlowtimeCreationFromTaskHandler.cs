using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeCreationFromTaskHandler : IRequestHandler<FlowtimeCreationFromTaskRequest, FlowtimeCreationFromTaskResponse>
{
    private readonly Repository _repository;

    public FlowtimeCreationFromTaskHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<FlowtimeCreationFromTaskResponse> Handle(FlowtimeCreationFromTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId);

        if (task.Description != request.TaskDescription)
        {
            task.ChangeDescription(request.TaskDescription);

            await _repository.Save(task, request.TaskVersion ?? -1);

            task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId);
        }

        var flowtime = new Flowtime(task);

        await _repository.Save(flowtime, -1);

        var response = new FlowtimeCreationFromTaskResponse(request.GetCorrelationId())
        {
            FlowtimeId = flowtime.Id,
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
