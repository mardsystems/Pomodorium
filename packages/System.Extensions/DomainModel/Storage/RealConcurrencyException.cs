namespace System.DomainModel.Storage;

public class RealConcurrencyException : Exception
{
    public RealConcurrencyException(EventStoreConcurrencyException ex)
        : base(null, ex)
    {

    }
}
