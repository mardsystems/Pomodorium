namespace System.DomainModel.EventStore;

public interface IAppendOnlyStore : IDisposable
{
    EventRecord Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = -1);

    void Append(EventRecord tapeRecord);

    IEnumerable<EventRecord> ReadRecords(string name, long afterVersion, long maxCount);

    IEnumerable<EventRecord> ReadRecords(long afterVersion, long maxCount);

    void Close();
}
