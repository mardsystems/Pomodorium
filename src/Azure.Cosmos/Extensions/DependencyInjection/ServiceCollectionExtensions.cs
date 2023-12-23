using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomodorium.Data;
using Pomodorium.Features.Maintenance;
using Pomodorium.Features.Settings;
using Pomodorium.Features.Storage;
using Pomodorium.Repositories;
using System.DomainModel.Storage;
using System.Reflection;

namespace Pomodorium.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCosmos(this IServiceCollection services, IConfiguration configuration)
    {
        var endpointUri = configuration.GetConnectionString("EndpointUri") ?? throw new InvalidOperationException("EndpointUri not found.");
        var primaryKey = configuration.GetConnectionString("PrimaryKey") ?? throw new InvalidOperationException("PrimaryKey not found.");

        services.AddOptions<CosmosOptions>()
            .Bind(configuration.GetSection(CosmosOptions.CONFIGURATION_SECTION_NAME));

        services.AddSingleton((_) =>
        {
            var client = new CosmosClient(endpointUri, primaryKey);

            return client;
        });

        services.AddScoped<IAppendOnlyStore, CosmosStore>(factory =>
            new CosmosStore(
                new CosmosClient(endpointUri, primaryKey),
                factory.GetRequiredService<IOptions<CosmosOptions>>(),
                factory.GetRequiredService<ILogger<CosmosStore>>()));

        services.AddScoped<ITfsIntegrationRepository, CosmosTfsIntegrationService>();

        services.AddScoped<ITrelloIntegrationRepository, CosmosTrelloIntegrationService>();

        services.AddScoped<IReadOnlyDatabase, ReadOnlyDatabase>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
