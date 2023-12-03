using Pomodorium.Enums;

namespace Pomodorium.Models.TaskManagement.Integrations;

public class TaskInfo
{
    public IntegrationTypeEnum IntegrationType { get; }

    public Guid IntegrationId { get; }

    public string IntegrationName { get; }

    public string Reference { get; }

    public string Name { get; }

    public TaskInfo(
        IntegrationTypeEnum integrationType,
        Guid integrationId,
        string integrationName,
        string reference,
        string name)
    {
        IntegrationType = integrationType;
        IntegrationId = integrationId;
        IntegrationName = integrationName;
        Reference = reference;
        Name = name;
    }
}
