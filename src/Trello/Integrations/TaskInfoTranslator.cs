using Pomodorium.Enums;
using Pomodorium.Models;
using TaskManagement.Models.Integrations;
using Trello.Models;

namespace Trello.Integrations;

public static class TaskInfoTranslator
{
    public static TaskInfo ToTaskInfo(this Card card, IntegrationBase integrationBase)
    {
        if (integrationBase.Id == null)
        {
            throw new InvalidOperationException();
        }

        if (integrationBase.Name == null)
        {
            throw new InvalidOperationException();
        }

        if (card.id == null)
        {
            throw new InvalidOperationException();
        }

        var cardId = card.id;

        return new TaskInfo(
            IntegrationTypeEnum.Trello,
            integrationBase.Id.Value,
            integrationBase.Name,
            card.id,
            $"{card.name} (#{cardId.Substring(cardId.Length - 4, 4)})");
    }
}
