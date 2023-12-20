using Pomodorium.Enums;
using System.Runtime.Serialization;

namespace Pomodorium.Models.TaskManagement.Integrations;

[DataContract]
public class TaskIntegrated : Event
{
    [DataMember(Order = 1)]
    public Guid TaskIntegrationId { get; private set; }

    [DataMember(Order = 2)]
    public Guid TaskId { get; private set; }

    [DataMember(Order = 3)]
    public IntegrationTypeEnum IntegrationType { get; private set; }

    [DataMember(Order = 4)]
    public Guid IntegrationId { get; private set; }

    [DataMember(Order = 5)]
    public string IntegrationName { get; private set; } = default!;

    [DataMember(Order = 6)]
    public string ExternalReference { get; private set; } = default!;

    public TaskIntegrated(
        Guid taskIntegrationId,
        Guid taskId,
        IntegrationTypeEnum integrationType,
        Guid integrationId,
        string integrationName,
        string externalReference)
    {
        TaskIntegrationId = taskIntegrationId;

        TaskId = taskId;

        IntegrationType = integrationType;

        IntegrationId = integrationId;

        IntegrationName = integrationName;

        ExternalReference = externalReference;
    }

    private TaskIntegrated() { }
}
