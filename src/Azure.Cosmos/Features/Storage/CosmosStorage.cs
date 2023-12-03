using System;
using System.Collections.Generic;
using System.DomainModel.Storage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodorium.Features.Storage
{
    public class CosmosStorage : IAppendOnlyStore
    {
        public Task<EventRecord> Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = -1)
        {
            throw new NotImplementedException();
        }

        public Task Append(EventRecord tapeRecord)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventRecord>> ReadRecords(long maxCount)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventRecord>> ReadRecords(string name, long afterVersion, long maxCount)
        {
            throw new NotImplementedException();
        }
    }
}
