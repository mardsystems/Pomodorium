using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomodorium.Integrations;
using Pomodorium.Integrations.Trello;

namespace Pomodorium.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTrelloIntegration(this IServiceCollection services, IConfiguration configuration)
        {
            var trelloConfigurationSection = configuration.GetSection(TrelloIntegrationOptions.CONFIGURATION_SECTION_NAME);

            var trelloIntegrationOptions = trelloConfigurationSection.Get<TrelloIntegrationOptions>() ?? throw new InvalidOperationException();

            if (trelloIntegrationOptions.BaseAddress == null)
            {
                throw new InvalidOperationException();
            }

            services.AddOptions<TrelloIntegrationOptions>()
                .Bind(trelloConfigurationSection);

            services.AddHttpClient(TrelloIntegrationOptions.CONFIGURATION_SECTION_NAME, client =>
            {
                client.BaseAddress = new Uri(trelloIntegrationOptions.BaseAddress);
            });

            services.AddScoped<CardAdapter>();

            return services;
        }
    }
}
