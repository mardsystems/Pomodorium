using TechTalk.SpecFlow.Assist;

namespace Pomodorium.Support;

public class NullValueComparer : IValueComparer
{
    private readonly string _nullValue;
    public NullValueComparer(string nullValue)
    {
        _nullValue = nullValue;
    }

    public bool CanCompare(object actualValue)
    {
        return actualValue is null || actualValue is string;
    }

    public bool Compare(string expectedValue, object actualValue)
    {
        if (_nullValue == expectedValue)
        {
            return actualValue == null;
        }
        return expectedValue == (string)actualValue;
    }
}
