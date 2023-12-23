using MediatR;
using System.DomainModel.Storage;

namespace Pomodorium.Features.Maintenance;

public class IndexRebuildHandler : IRequestHandler<IndexRebuildRequest, IndexRebuildResponse>
{
    private readonly IMediator _mediator;

    private readonly IReadOnlyDatabase _readOnlyDatabaseInterface;

    public IndexRebuildHandler(
        IMediator mediator, IReadOnlyDatabase readOnlyDatabaseInterface)
    {
        _mediator = mediator;
        _readOnlyDatabaseInterface = readOnlyDatabaseInterface;
    }

    public async Task<IndexRebuildResponse> Handle(IndexRebuildRequest request, CancellationToken cancellationToken)
    {
        await _readOnlyDatabaseInterface.EnsureDeleted();

        await _readOnlyDatabaseInterface.EnsureCreated();

        var getEventsRequest = new GetEventsRequest { };

        var getEventsResponse = await _mediator.Send<GetEventsResponse>(getEventsRequest, cancellationToken);

        var eventRecords = getEventsResponse.EventRecords; //.OrderBy(x => x.Date);

        foreach (var eventRecord in eventRecords)
        {
            var type = Type.GetType(eventRecord.TypeName) ?? throw new InvalidOperationException();

            var @event = EventStore.DesserializeEvent(type, eventRecord.Data);

            await _mediator.Publish(@event, cancellationToken);
        }

        var response = new IndexRebuildResponse(request.GetCorrelationId());

        return response;
    }
}
