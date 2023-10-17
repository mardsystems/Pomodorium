using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DomainModel.EventStore;

public class EventRecord
{
    public string Name { get; }

    public long Version { get; }

    public DateTime Date { get; }

    public byte[] Data { get; }

    public EventRecord(string name, long version, DateTime date, byte[] data)
    {
        Name = name;

        Version = version;

        Date = date;

        Data = data;
    }

    private EventRecord()
    {

    }
}
