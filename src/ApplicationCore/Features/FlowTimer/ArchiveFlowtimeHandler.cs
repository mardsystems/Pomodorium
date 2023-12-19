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
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.FlowtimeId) ?? throw new EntityNotFoundException();

        flowtime.Archive();

        await _repository.Save(flowtime, request.FlowtimeVersion);

        var response = new ArchiveFlowtimeResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
