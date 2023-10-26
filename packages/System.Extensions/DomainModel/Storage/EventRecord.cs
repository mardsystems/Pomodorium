using MediatR;

namespace System.DomainModel.Storage;

public class EventRecord : IIdentity, INotification
{
    public string Name { get; }

    public string TypeName { get; }

    public long Version { get; }

    public DateTime Date { get; }

    public byte[] Data { get; }

    public EventRecord(string name, string typeName, long version, DateTime date, byte[] data)
    {
        Name = name;

        TypeName = typeName;

        Version = version;

        Date = date;

        Data = data;
    }

    public override string ToString()
    {
        return Name;
    }

    private EventRecord()
    {

    }
}
