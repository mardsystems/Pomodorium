using Microsoft.AspNetCore.SignalR;
using System.DomainModel.EventStore;

namespace Pomodorium.Hubs
{
    public class EventHub : Hub
    {
        private readonly IAppendOnlyStore appendOnlyStore;

        public EventHub(IAppendOnlyStore appendOnlyStore)
        {
            this.appendOnlyStore = appendOnlyStore;
        }

        public void Append(string name, DateTime date, byte[] data, long expectedVersion)
        {
            //appendOnlyStore.Append(name, date, data, expectedVersion);

            Clients.Others.SendAsync("Append", name, date, data, expectedVersion);
        }
    }
}
