namespace Pomodorium.Models;

public class TrelloIntegration : IntegrationBase
{
    public string? Key { get; set; }

    public string? Token { get; set; }

    public string? BoardId { get; set; }
}
