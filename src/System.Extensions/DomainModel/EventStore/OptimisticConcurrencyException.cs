namespace System.DomainModel.EventStore;

public class OptimisticConcurrencyException : Exception
{
    public static OptimisticConcurrencyException Create(long serverVersion, long expectedVersion, IIdentity id, IEnumerable<Event> serverEvents)
    {
        return new OptimisticConcurrencyException();
    }
}
