namespace Pomodorium.Modules.Flows;

public class ArchiveFlowtimeRequest : Request<ArchiveFlowtimeResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}
