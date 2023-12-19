using Pomodorium.Models;

namespace Pomodorium.Repositories;

public interface ITfsIntegrationRepository
{
    Task<IEnumerable<TfsIntegration>> GetTfsIntegrationList(TfsIntegration? criteria = null, CancellationToken cancellationToken = default);

    Task<TfsIntegration> GetTfsIntegration(Guid id, CancellationToken cancellationToken = default);

    Task<TfsIntegration> CreateTfsIntegration(TfsIntegration tfsIntegration, CancellationToken cancellationToken = default);

    Task<TfsIntegration> UpdateTfsIntegration(TfsIntegration tfsIntegration, CancellationToken cancellationToken = default);

    Task DeleteTfsIntegration(Guid id, CancellationToken cancellationToken = default);
}
