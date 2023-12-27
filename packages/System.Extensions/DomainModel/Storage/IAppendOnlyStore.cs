namespace System.DomainModel.Storage;

public interface IAppendOnlyStore : IDisposable
{
    Task<IEnumerable<EventRecord>> ReadRecords(long maxCount);

    Task<IEnumerable<EventRecord>> ReadRecords(string name, long afterVersion, long maxCount);

    Task<EventRecord> Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = EventStore.IRRELEVANT_VERSION);

    Task Append(EventRecord tapeRecord);

    void Close();
}
