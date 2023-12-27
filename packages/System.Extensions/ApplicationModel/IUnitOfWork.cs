using Microsoft.Extensions.Logging;

namespace System.ApplicationModel;

public interface IUnitOfWork
{
    TransactionContext BeginTransactionFor(Request request, ILogger logger);
}
