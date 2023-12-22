using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeInterruptionHandler : IRequestHandler<FlowtimeInterruptionRequest, FlowtimeInterruptionResponse>
{
    private readonly Repository _repository;

    public FlowtimeInterruptionHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<FlowtimeInterruptionResponse> Handle(FlowtimeInterruptionRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

        var now = DateTime.Now;

        flowtime.Interrupt(now);

        await _repository.Save(flowtime, request.FlowtimeVersion ?? -1);

        var response = new FlowtimeInterruptionResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
