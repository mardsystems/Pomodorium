using Pomodorium.FlowtimeTechnique;

namespace Pomodorium.TimeManagement.FlowTimer;

public class CreateFlowtimeRequest : Request<CreateFlowtimeResponse>
{
    public Guid? TaskId { get; set; }

    public string? TaskDescription { get; set; }

    public long? TaskVersion { get; set; }
}

public class CreateFlowtimeResponse : Response
{
    public CreateFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public CreateFlowtimeResponse() { }
}

public class CreateFlowtimeHandler : IRequestHandler<CreateFlowtimeRequest, CreateFlowtimeResponse>
{
    private readonly Repository _repository;

    public CreateFlowtimeHandler(Repository flowtimeRepository)
    {
        _repository = flowtimeRepository;
    }

    public async Task<CreateFlowtimeResponse> Handle(CreateFlowtimeRequest request, CancellationToken cancellationToken)
    {
        TaskManagement.Tasks.Task task;

        if (request.TaskId.HasValue)
        {
            task = await _repository.GetAggregateById<TaskManagement.Tasks.Task>(request.TaskId.Value);

            if (task.Description != request.TaskDescription)
            {
                task.ChangeDescription(request.TaskDescription);

                await _repository.Save(task, request.TaskVersion.Value);

                task = await _repository.GetAggregateById<TaskManagement.Tasks.Task>(request.TaskId.Value);
            }
        }
        else
        {
            task = new TaskManagement.Tasks.Task(request.TaskDescription);

            await _repository.Save(task, -1);
        }

        var flowtime = new Flowtime(task);

        await _repository.Save(flowtime, -1);

        var response = new CreateFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }
}