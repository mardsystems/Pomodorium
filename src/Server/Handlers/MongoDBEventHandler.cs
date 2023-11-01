using MediatR;
using Pomodorium.Data;
using Pomodorium.Events;
using System.DomainModel.Storage;

namespace Pomodorium.Handlers;

public class MongoDBEventHandler : IRequestHandler<GetEventsRequest, GetEventsResponse>
{
    private readonly IAppendOnlyStore _storage;

    public MongoDBEventHandler(IAppendOnlyStore storage)
    {
        _storage = storage;
    }

    public async Task<GetEventsResponse> Handle(GetEventsRequest request, CancellationToken cancellationToken)
    {
        var records = await _storage.ReadRecords(long.MaxValue);

        var response = new GetEventsResponse { EventRecords = records };

        return response;
    }
}
