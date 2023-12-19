using Newtonsoft.Json;
using Pomodorium.Enums;

namespace Pomodorium.Features.TaskManager;

public record GetTasksRequest : Request<GetTasksResponse>
{
    public int? PageSize { get; init; }

    public int? PageIndex { get; init; }

    public string? Description { get; init; }

    public string? ExternalReference { get; init; }
}

public record GetTasksResponse(Guid CorrelationId) : Response(CorrelationId)
{
    public required IEnumerable<TaskQueryItem> TaskQueryItems { get; init; }
}

public class TaskQueryItem
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? Description { get; set; }

    public double TotalHours { get; set; }

    public IntegrationTypeEnum? IntegrationType { get; set; }

    public Guid? IntegrationId { get; set; }

    public string? IntegrationName { get; set; }

    public string? ExternalReference { get; set; }

    public bool? HasFocus { get; set; }

    public long Version { get; set; }
}
