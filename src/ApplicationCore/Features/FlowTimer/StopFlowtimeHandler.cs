using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class StopFlowtimeHandler : IRequestHandler<StopFlowtimeRequest, StopFlowtimeResponse>
{
    private readonly Repository _repository;

    public StopFlowtimeHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<StopFlowtimeResponse> Handle(StopFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

        var now = DateTime.Now;

        flowtime.Stop(now);

        await _repository.Save(flowtime, request.FlowtimeVersion);

        var response = new StopFlowtimeResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
