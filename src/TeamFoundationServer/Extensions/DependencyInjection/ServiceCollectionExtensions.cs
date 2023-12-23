using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomodorium.Integrations;
using Pomodorium.Integrations.TFS;

namespace Pomodorium.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTfsIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TfsIntegrationOptions>()
            .Bind(configuration.GetSection(TfsIntegrationOptions.CONFIGURATION_SECTION_NAME));

        services.AddScoped<WorkItemAdapter>();

        return services;
    }
}
