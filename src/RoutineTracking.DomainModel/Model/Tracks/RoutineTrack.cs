namespace Pomodorium.OfficeTimeTracking.Model.Tracks;

public abstract class RoutineTrack : AggregateRoot
{
    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }
}
