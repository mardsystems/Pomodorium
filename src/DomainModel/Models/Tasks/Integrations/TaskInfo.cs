﻿using Pomodorium.Enums;

namespace Pomodorium.Models.Tasks.Integrations;

public class TaskInfo
{
    public IntegrationTypeEnum IntegrationType { get; }

    public Guid IntegrationId { get; }

    public string IntegrationName { get; } = default!;

    public string Reference { get; } = default!;

    public string Name { get; } = default!;

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
