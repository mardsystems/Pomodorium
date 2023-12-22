using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeArchivingHandler : IRequestHandler<FlowtimeArchivingRequest, FlowtimeArchivingResponse>
{
    private readonly Repository _repository;

    public FlowtimeArchivingHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<FlowtimeArchivingResponse> Handle(FlowtimeArchivingRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

        flowtime.Archive();

        await _repository.Save(flowtime, request.FlowtimeVersion ?? -1);

        var response = new FlowtimeArchivingResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
