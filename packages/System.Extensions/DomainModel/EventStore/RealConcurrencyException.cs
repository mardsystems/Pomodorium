namespace System.DomainModel.EventStore;

public class RealConcurrencyException : Exception
{
    public RealConcurrencyException(EventStoreConcurrencyException ex)
        : base(null, ex)
    {

    }
}
