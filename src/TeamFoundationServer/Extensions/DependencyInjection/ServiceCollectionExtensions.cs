using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomodorium.Models.Tasks.Integrations;
using TeamFoundationServer.Integrations;

namespace TeamFoundationServer.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTfsIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TfsIntegrationOptions>()
            .Bind(configuration.GetSection(TfsIntegrationOptions.CONFIGURATION_SECTION_NAME));

        services.AddScoped<ITfsIntegrationService, TfsIntegrationService>();

        services.AddScoped<WorkItemAdapter>();
        
        services.AddScoped<TfsFacade>();

        return services;
    }
}
