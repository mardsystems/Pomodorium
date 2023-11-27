using Pomodorium.FlowtimeTechnique;

namespace Pomodorium.TimeManagement.FlowTimer;

public class StopFlowtimeRequest : Request<StopFlowtimeResponse>
{
    public Guid Id { get; set; }

    public DateTime StopDateTime { get; set; }

    public long Version { get; set; }
}

public class StopFlowtimeResponse : Response
{
    public StopFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public StopFlowtimeResponse() { }
}

public class StopFlowtimeHandler : IRequestHandler<StopFlowtimeRequest, StopFlowtimeResponse>
{
    private readonly Repository _repository;

    public StopFlowtimeHandler(Repository flowtimeRepository)
    {
        _repository = flowtimeRepository;
    }

    public async Task<StopFlowtimeResponse> Handle(StopFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.Id);

        if (flowtime == null)
        {
            throw new EntityNotFoundException();
        }

        flowtime.Stop(request.StopDateTime);

        await _repository.Save(flowtime, request.Version);

        var response = new StopFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }
}