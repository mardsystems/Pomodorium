﻿using MongoDB.Bson.Serialization.Attributes;

namespace Pomodorium.Features.TaskSynchronizer;

public class TfsIntegration
{
    [BsonId]
    public Guid Id { get; set; }

    public string Name { get; set; }

    /// <param name="orgName">
    ///     An organization in Azure DevOps Services. If you don't have one, you can create one for free:
    ///     <see href="https://go.microsoft.com/fwlink/?LinkId=307137" />.
    /// </param>
    public string OrganizationName { get; set; }

    /// <param name="personalAccessToken">
    ///     A Personal Access Token, find out how to create one:
    ///     <see href="/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops" />.
    /// </param>
    public string PersonalAccessToken { get; set; }

    public string ProjectName { get; set; }
}
