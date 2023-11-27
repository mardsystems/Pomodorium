using Pomodorium.FlowtimeTechnique;

namespace Pomodorium.TimeManagement.FlowTimer;

public class InterruptFlowtimeRequest : Request<InterruptFlowtimeResponse>
{
    public Guid Id { get; set; }

    public DateTime InterruptDateTime { get; set; }

    public long Version { get; set; }
}

public class InterruptFlowtimeResponse : Response
{
    public InterruptFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public InterruptFlowtimeResponse() { }
}

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