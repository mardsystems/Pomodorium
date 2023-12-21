using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeStopHandler : IRequestHandler<FlowtimeStopRequest, FlowtimeStopResponse>
{
    private readonly Repository _repository;

    public FlowtimeStopHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<FlowtimeStopResponse> Handle(FlowtimeStopRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

        var now = DateTime.Now;

        flowtime.Stop(now);

        await _repository.Save(flowtime, request.FlowtimeVersion);

        var response = new FlowtimeStopResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
