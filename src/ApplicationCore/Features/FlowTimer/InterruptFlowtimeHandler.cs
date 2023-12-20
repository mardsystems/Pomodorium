using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class InterruptFlowtimeHandler : IRequestHandler<InterruptFlowtimeRequest, InterruptFlowtimeResponse>
{
    private readonly Repository _repository;

    public InterruptFlowtimeHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<InterruptFlowtimeResponse> Handle(InterruptFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

        var now = DateTime.Now;

        flowtime.Interrupt(now);

        await _repository.Save(flowtime, request.FlowtimeVersion);

        var response = new InterruptFlowtimeResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
