using Microsoft.Extensions.Logging;
using System.DomainModel;

namespace System.ApplicationModel;

public class TransactionContext : AuditInterface
{
    public Guid CorrelationId { get; }

    public string UserId { get; }

    public DateTime StartedAt { get; }

    private readonly ILogger _logger;

    public TransactionContext(Guid correlationId, string userId, DateTime startedAt, ILogger logger)
    {
        CorrelationId = correlationId;

        UserId = userId;

        StartedAt = startedAt;

        _logger = logger;
    }

    public void Commit()
    {
        _logger.LogInformation("Commit");
    }

    public void Rollback()
    {
        _logger.LogInformation("Rollback");
    }

    public void Rollback(Exception ex)
    {
        _logger.LogError(ex, "Rollback");
    }

    string AuditInterface.GetUserId() => UserId;

    DateTime AuditInterface.GetCreationDate() => StartedAt;
}
