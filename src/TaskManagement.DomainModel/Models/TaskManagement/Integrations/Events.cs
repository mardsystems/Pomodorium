using Pomodorium.Enums;
using System.Runtime.Serialization;

namespace Pomodorium.Models.TaskManagement.Integrations;

[DataContract]
public class TaskIntegrated : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public Guid TaskId { get; private set; }

    [DataMember(Order = 3)]
    public IntegrationTypeEnum IntegrationType { get; private set; }

    [DataMember(Order = 4)]
    public Guid IntegrationId { get; private set; }

    [DataMember(Order = 5)]
    public string IntegrationName { get; private set; }

    [DataMember(Order = 6)]
    public string ExternalReference { get; private set; }

    public TaskIntegrated(
        Guid id,
        Guid taskId,
        IntegrationTypeEnum integrationType,
        Guid integrationId,
        string integrationName,
        string externalReference)
    {
        Id = id;

        TaskId = taskId;

        IntegrationType = integrationType;

        IntegrationId = integrationId;

        IntegrationName = integrationName;

        ExternalReference = externalReference;
    }

    private TaskIntegrated() { }
}
