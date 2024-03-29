﻿using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace System.DomainModel.Storage;

public class EventRecord
{
    [BsonId]
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; }

    public string Name { get; } = default!;

    public long Version { get; }

    public DateTime Date { get; }

    public string TypeName { get; } = default!;

    public byte[] Data { get; } = default!;

    public EventRecord(string name, long version, DateTime date, string typeName, byte[] data)
    {
        Id = Guid.NewGuid();

        Name = name;

        Version = version;

        Date = date;

        TypeName = typeName;

        Data = data;
    }

    private EventRecord() { }
}
