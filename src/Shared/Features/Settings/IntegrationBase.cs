using MongoDB.Bson.Serialization.Attributes;

namespace Pomodorium.Features.Settings;

public abstract class IntegrationBase
{
    [BsonId]
    public Guid Id { get; set; }

    public string Name { get; set; }
}
