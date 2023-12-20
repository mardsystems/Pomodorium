namespace System.DomainModel.Storage;

public class EventStoreConcurrencyException : Exception
{
    /// <summary>
    /// Actual Events.
    /// </summary>
    public required Event[] StoreEvents { get; set; }

    /// <summary>
    /// Actual Version.
    /// </summary>
    public long StoreVersion { get; set; }
}
