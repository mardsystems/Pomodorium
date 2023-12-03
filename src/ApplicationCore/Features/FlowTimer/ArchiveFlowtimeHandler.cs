using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class ArchiveFlowtimeHandler : IRequestHandler<ArchiveFlowtimeRequest, ArchiveFlowtimeResponse>
{
    private readonly Repository _repository;

    public ArchiveFlowtimeHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<ArchiveFlowtimeResponse> Handle(ArchiveFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.Id);

        if (flowtime == null)
        {
            throw new EntityNotFoundException();
        }

        flowtime.Archive();

        await _repository.Save(flowtime, request.Version);

        var response = new ArchiveFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }
}
