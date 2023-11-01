using System.DomainModel.Storage;

namespace Pomodorium.Hubs;

public interface IHubEvent
{
    Task Notify(EventAppended notification);
}
