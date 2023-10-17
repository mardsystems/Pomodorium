using MediatR;

namespace System.DomainModel;

[Serializable]
public abstract class Event : INotification
{
    [NonSerialized]
    private bool isHandled;
    public bool IsHandled
    {
        get { return isHandled; }
        set
        {
            isHandled = value;
        }
    }

    [NonSerialized]
    private long version;
    public long Version
    {
        get { return version; }
        set
        {
            version = value;
        }
    }

    [NonSerialized]
    private DateTime date;
    public DateTime Date
    {
        get { return date; }
        set
        {
            date = value;
        }
    }
}
