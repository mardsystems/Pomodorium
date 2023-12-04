using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Pomodorium.Models;

public abstract class IntegrationBase
{
    [BsonId]
    [JsonProperty(PropertyName = "id")]
    public Guid? Id { get; set; }

    public string? Name { get; set; }
}
