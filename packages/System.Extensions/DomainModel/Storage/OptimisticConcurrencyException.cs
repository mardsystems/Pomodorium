namespace System.DomainModel.Storage;

public class OptimisticConcurrencyException : Exception
{
    public static OptimisticConcurrencyException Create(long serverVersion, long expectedVersion, IIdentity id, IEnumerable<Event> serverEvents)
    {
        return new OptimisticConcurrencyException();
    }
}
