using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace System.DomainModel.Storage;

public class EventRecord : INotification
{
    [BsonId]
    public Guid Id { get; }

    public string Name { get; }

    public long Version { get; }

    public DateTime Date { get; }

    public string TypeName { get; }

    public byte[] Data { get; }

    public EventRecord(string name, long version, DateTime date, string typeName, byte[] data)
    {
        Id = Guid.NewGuid();

        Name = name;

        Version = version;

        Date = date;

        TypeName = typeName;

        Data = data;
    }

    private EventRecord()
    {

    }
}
