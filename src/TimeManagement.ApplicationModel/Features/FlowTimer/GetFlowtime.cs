using Pomodorium.FlowtimeTechnique.Model;
using System.ComponentModel.DataAnnotations;

namespace Pomodorium.Features.FlowTimer;

public class GetFlowtimeRequest : Request<GetFlowtimeResponse>
{
    public Guid Id { get; set; }
}

public class GetFlowtimeResponse : Response
{
    public GetFlowtimeResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public FlowtimeDetails FlowtimeDetails { get; set; }

    public GetFlowtimeResponse()
    {

    }
}

public class FlowtimeDetails
{
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public Guid TaskId { get; set; }

    public string? TaskDescription { get; set; }

    public long TaskVersion { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public bool? Interrupted { get; set; }

    [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}")]
    public TimeSpan? Worktime { get; set; }

    public TimeSpan? Breaktime { get; set; }

    public TimeSpan? ExpectedDuration { get; set; }

    public FlowtimeState? State { get; set; }

    public long Version { get; set; }
}
