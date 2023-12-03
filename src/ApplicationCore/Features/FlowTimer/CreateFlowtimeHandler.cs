using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class CreateFlowtimeHandler : IRequestHandler<CreateFlowtimeRequest, CreateFlowtimeResponse>
{
    private readonly Repository _repository;

    public CreateFlowtimeHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<CreateFlowtimeResponse> Handle(CreateFlowtimeRequest request, CancellationToken cancellationToken)
    {
        Models.TaskManagement.Tasks.Task task;

        if (request.TaskId.HasValue)
        {
            task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId.Value);

            if (task.Description != request.TaskDescription)
            {
                task.ChangeDescription(request.TaskDescription);

                await _repository.Save(task, request.TaskVersion.Value);

                task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId.Value);
            }
        }
        else
        {
            task = new Models.TaskManagement.Tasks.Task(request.TaskDescription);

            await _repository.Save(task, -1);
        }

        var flowtime = new Flowtime(task);

        await _repository.Save(flowtime, -1);

        var response = new CreateFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }
}
