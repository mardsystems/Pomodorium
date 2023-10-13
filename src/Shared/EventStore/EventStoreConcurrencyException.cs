using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodorium.EventStore;

public class EventStoreConcurrencyException : Exception
{
    /// <summary>
    /// Actual Events.
    /// </summary>
    public Event[] StoreEvents { get; set; }

    /// <summary>
    /// Actual Version.
    /// </summary>
    public long StoreVersion { get; set; }
}
