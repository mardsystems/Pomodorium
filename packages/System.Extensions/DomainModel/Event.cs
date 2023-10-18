using MediatR;
using System.Runtime.Serialization;

namespace System.DomainModel;

//[DataContract]
public abstract class Event : INotification
{
    [IgnoreDataMember]
    private bool isHandled;
    public bool IsHandled
    {
        get { return isHandled; }
        set
        {
            isHandled = value;
        }
    }

    [IgnoreDataMember]
    private string name;
    public string Name
    {
        get { return name; }
        set
        {
            name = value;
        }
    }

    [IgnoreDataMember]
    private long version;
    public long Version
    {
        get { return version; }
        set
        {
            version = value;
        }
    }

    [IgnoreDataMember]
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
