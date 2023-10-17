using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DomainModel.EventStore;

public interface IAppendOnlyStore : IDisposable
{
    void Append(string name, DateTime date, byte[] data, long expectedVersion = -1);

    IEnumerable<EventRecord> ReadRecords(string name, long afterVersion, long maxCount);

    IEnumerable<EventRecord> ReadRecords(long afterVersion, long maxCount);

    void Close();
}
