namespace System.DomainModel.EventStore;

public class AppendOnlyStoreConcurrencyException : Exception
{
    public long Version { get; }

    public long ExpectedVersion { get; }

    public string Name { get; }

    public AppendOnlyStoreConcurrencyException(long version, long expectedVersion, string name)
    {
        Version = version;

        ExpectedVersion = expectedVersion;

        Name = name;
    }
}
