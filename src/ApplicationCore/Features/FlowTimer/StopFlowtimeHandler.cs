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
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.Id);

        if (flowtime == null)
        {
            throw new EntityNotFoundException();
        }

        var now = DateTime.Now;

        flowtime.Stop(now);

        await _repository.Save(flowtime, request.Version);

        var response = new StopFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }
}
