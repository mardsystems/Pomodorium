#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

using Pomodorium.Features.FlowTimer;

namespace Pomodorium.Data;

class FlowtimeQueryItemComparer : IEqualityComparer<FlowtimeQueryItem>
{
    public bool Equals(FlowtimeQueryItem a, FlowtimeQueryItem b) => a?.Id == b?.Id;
    public int GetHashCode(FlowtimeQueryItem x) => HashCode.Combine(x?.Id);
}
