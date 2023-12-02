using MediatR;
using MongoDB.Driver;

namespace Pomodorium.Features.Settings;

public class MongoDBTfsIntegrationCollection :
    IRequestHandler<GetTfsIntegrationListRequest, GetTfsIntegrationListResponse>,
    IRequestHandler<CreateTfsIntegrationRequest, CreateTfsIntegrationResponse>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<TfsIntegration> _mongoCollection;

    public MongoDBTfsIntegrationCollection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<TfsIntegration>("TfsIntegrationCollection");
    }

    public async Task<GetTfsIntegrationListResponse> Handle(GetTfsIntegrationListRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<TfsIntegration>.Filter.Empty;

        var tfsIntegrationList = await _mongoCollection.Find(filter).ToListAsync(cancellationToken);

        var response = new GetTfsIntegrationListResponse(request.GetCorrelationId()) { TfsIntegrationList = tfsIntegrationList };

        return response;
    }

    public async Task<CreateTfsIntegrationResponse> Handle(CreateTfsIntegrationRequest request, CancellationToken cancellationToken)
    {
        var tfsIntegrationInfo = new TfsIntegration
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            OrganizationName = request.OrganizationName,
            PersonalAccessToken = request.PersonalAccessToken,
            ProjectName = request.ProjectName
        };

        await _mongoCollection.InsertOneAsync(tfsIntegrationInfo, null, cancellationToken);

        var response = new CreateTfsIntegrationResponse(request.GetCorrelationId()) { };

        return response;
    }
}
