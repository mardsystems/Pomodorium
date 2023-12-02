using Pomodorium.Enums;
using Pomodorium.Models;
using Pomodorium.TaskManagement.Model.Integrations;

namespace Pomodorium.Trello;

public static class TaskInfoTranslator
{
    public static TaskInfo ToTaskInfo(this Card card, IntegrationBase integrationBase)
    {
        var cardId = card.id;

        return new TaskInfo(
            IntegrationTypeEnum.Trello,
            integrationBase.Id.Value,
            integrationBase.Name,
            card.id,
            $"{card.name} (#{cardId.Substring(cardId.Length - 4, 4)})");
    }
}
