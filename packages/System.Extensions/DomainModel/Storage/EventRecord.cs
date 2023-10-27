using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace System.DomainModel.Storage;

public class EventRecord : INotification
{
    [BsonId]
    public ObjectId Id { get; }

    public string Name { get; }

    public string TypeName { get; }

    public long Version { get; }

    public DateTime Date { get; }

    public byte[] Data { get; }

    public EventRecord(string name, string typeName, long version, DateTime date, byte[] data)
    {
        Id = ObjectId.GenerateNewId();

        Name = name;

        TypeName = typeName;

        Version = version;

        Date = date;

        Data = data;
    }

    private EventRecord()
    {

    }
}
