using Pomodorium.FlowtimeTechnique.Model;

namespace Pomodorium.Features.FlowTimer;

public class InterruptFlowtimeHandler : IRequestHandler<InterruptFlowtimeRequest, InterruptFlowtimeResponse>
{
    private readonly Repository _repository;

    public InterruptFlowtimeHandler(Repository flowtimeRepository)
    {
        _repository = flowtimeRepository;
    }

    public async Task<InterruptFlowtimeResponse> Handle(InterruptFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.Id);

        if (flowtime == null)
        {
            throw new EntityNotFoundException();
        }

        flowtime.Interrupt(request.InterruptDateTime);

        await _repository.Save(flowtime, request.Version);

        var response = new InterruptFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }
}
