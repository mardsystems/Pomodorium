using MediatR;
using System.DomainModel.Storage;

namespace Pomodorium.Features.Storage;

public class GetEventsHandler : IRequestHandler<GetEventsRequest, GetEventsResponse>
{
    private readonly IAppendOnlyStore _storage;

    public GetEventsHandler(IAppendOnlyStore storage)
    {
        _storage = storage;
    }

    public async Task<GetEventsResponse> Handle(GetEventsRequest request, CancellationToken cancellationToken)
    {
        var records = await _storage.ReadRecords(long.MaxValue);

        var response = new GetEventsResponse(request.GetCorrelationId())
        {
            EventRecords = records
        };

        return response;
    }
}
