using Pomodorium.Features.Settings;

namespace Pomodorium.Features.TaskManager;

public class GetTasksRequest : Request<GetTasksResponse>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }

    public string? Description { get; set; }

    public string? ExternalReference { get; set; }
}

public class GetTasksResponse : Response
{
    public GetTasksResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<TaskQueryItem> TaskQueryItems { get; set; }

    public GetTasksResponse() { }
}

public class TaskQueryItem
{
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? Description { get; set; }

    public double? TotalHours { get; set; }

    public IntegrationType? IntegrationType { get; set; }

    public Guid? IntegrationId { get; set; }

    public string? IntegrationName { get; set; }

    public string? ExternalReference { get; set; }

    public long Version { get; set; }
}
