namespace Pomodorium.TeamFoundationServer;

public class TeamFoundationServerOptions
{
    public const string CONFIGURATION_SECTION_NAME = "TeamFoundationServer";

    public string OrganizationName { get; set; }

    public string PersonalAccessToken { get; set; }
}
