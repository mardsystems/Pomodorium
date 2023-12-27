using Microsoft.Extensions.Logging;

namespace System.ApplicationModel;

public class DefaultUnitOfWork : IUnitOfWork
{
    public TransactionContext BeginTransactionFor(Request request, ILogger logger)
    {
        var correlationId = request.GetCorrelationId();

        var userId = Thread.CurrentPrincipal?.Identity?.Name ?? string.Empty;

        var startedAt = DateTime.Now;

        var transaction = new TransactionContext(correlationId, userId, startedAt, logger);

        logger.LogInformation("Begin Transaction: {CorrelationId}", correlationId);

        return transaction;
    }
}
