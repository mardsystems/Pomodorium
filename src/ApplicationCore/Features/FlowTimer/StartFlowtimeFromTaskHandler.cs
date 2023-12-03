using Pomodorium.FlowtimeTechnique.Model;

namespace Pomodorium.Features.FlowTimer;

public class StartFlowtimeFromTaskHandler : IRequestHandler<StartFlowtimeFromTaskRequest, StartFlowtimeFromTaskResponse>
{
    private readonly Repository _repository;

    public StartFlowtimeFromTaskHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<StartFlowtimeFromTaskResponse> Handle(StartFlowtimeFromTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetAggregateById<TaskManagement.Model.Tasks.Task>(request.TaskId);

        var flowtime = new Flowtime(task);

        var now = DateTime.Now;

        flowtime.Start(now);

        await _repository.Save(flowtime, -1);

        var response = new StartFlowtimeFromTaskResponse(request.GetCorrelationId()) { };

        return response;
    }
}
