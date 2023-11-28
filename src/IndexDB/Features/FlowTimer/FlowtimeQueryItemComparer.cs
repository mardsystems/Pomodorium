namespace Pomodorium.Features.FlowTimer;

class FlowtimeQueryItemComparer : IEqualityComparer<FlowtimeQueryItem>
{
    public bool Equals(FlowtimeQueryItem a, FlowtimeQueryItem b) => a?.Id == b?.Id;
    public int GetHashCode(FlowtimeQueryItem x) => HashCode.Combine(x?.Id);
}
