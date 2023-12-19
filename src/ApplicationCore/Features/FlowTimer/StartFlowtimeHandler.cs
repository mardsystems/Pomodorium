using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class StartFlowtimeHandler : IRequestHandler<StartFlowtimeRequest, StartFlowtimeResponse>
{
    private readonly Repository _repository;

    public StartFlowtimeHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<StartFlowtimeResponse> Handle(StartFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

        var now = DateTime.Now;

        flowtime.Start(now);

        await _repository.Save(flowtime, request.FlowtimeVersion);

        var response = new StartFlowtimeResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
