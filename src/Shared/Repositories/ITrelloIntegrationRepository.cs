using Pomodorium.Models;

namespace Pomodorium.Repositories;

public interface ITrelloIntegrationRepository
{
    Task<IEnumerable<TrelloIntegration>> GetTrelloIntegrationList(TrelloIntegration criteria = null, CancellationToken cancellationToken = default);

    Task<TrelloIntegration> GetTrelloIntegration(Guid id, CancellationToken cancellationToken = default);

    Task<TrelloIntegration> CreateTrelloIntegration(TrelloIntegration trelloIntegration, CancellationToken cancellationToken = default);
    
    Task<TrelloIntegration> UpdateTrelloIntegration(TrelloIntegration trelloIntegration, CancellationToken cancellationToken = default);

    Task DeleteTrelloIntegration(Guid id, CancellationToken cancellationToken = default);
}