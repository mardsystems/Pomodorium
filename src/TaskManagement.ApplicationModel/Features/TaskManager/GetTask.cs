using Newtonsoft.Json;

namespace Pomodorium.Features.TaskManager;

public record GetTaskRequest(Guid TaskId) : Request<GetTaskResponse>
{
    
}

public record GetTaskResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required TaskDetails TaskDetails { get; init; }
}

public class TaskDetails
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? Description { get; set; }

    public long Version { get; set; }
}
