namespace Pomodorium;

[Serializable]

public abstract class Event
{
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
