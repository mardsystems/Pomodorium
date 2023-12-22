using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeStartHandler : IRequestHandler<FlowtimeStartRequest, FlowtimeStartResponse>
{
    private readonly Repository _repository;

    public FlowtimeStartHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<FlowtimeStartResponse> Handle(FlowtimeStartRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

        var now = DateTime.Now;

        flowtime.Start(now);

        await _repository.Save(flowtime, request.FlowtimeVersion ?? -1);

        var response = new FlowtimeStartResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
