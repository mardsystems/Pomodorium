using Pomodorium.Features.Settings;

namespace Pomodorium.TaskManagement.Model.Integrations;

public class TaskInfo
{
    public IntegrationType IntegrationType { get; }

    public Guid IntegrationId { get; }

    public string IntegrationName { get; }

    public string Reference { get; }

    public string Name { get; }

    public TaskInfo(
        IntegrationType integrationType,
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
