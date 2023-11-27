using Pomodorium.FlowtimeTechnique;

namespace Pomodorium.TimeManagement.FlowTimer;

public class StartFlowtimeRequest : Request<StartFlowtimeResponse>
{
    public Guid Id { get; set; }

    public DateTime StartDateTime { get; set; }

    public long Version { get; set; }
}

public class StartFlowtimeResponse : Response
{
    public StartFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public StartFlowtimeResponse() { }
}

public class StartFlowtimeHanlder : IRequestHandler<StartFlowtimeRequest, StartFlowtimeResponse>
{
    private readonly Repository _repository;

    public StartFlowtimeHanlder(Repository flowtimeRepository)
    {
        _repository = flowtimeRepository;
    }

    public async Task<StartFlowtimeResponse> Handle(StartFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.Id);

        if (flowtime == null)
        {
            throw new EntityNotFoundException();
        }

        flowtime.Start(request.StartDateTime);

        await _repository.Save(flowtime, request.Version);

        var response = new StartFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }
}