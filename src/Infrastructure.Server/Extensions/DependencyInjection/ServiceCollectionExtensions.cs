using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ApplicationModel;
using System.Reflection;
using TeamFoundationServer.Extensions.DependencyInjection;
using Trello.Extensions.DependencyInjection;

namespace Pomodorium.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServerInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSharedInfrastructure();

        services.AddTransient<IUnitOfWork, DefaultUnitOfWork>();

        services.AddCosmos(configuration);

        //services.AddMongoDB(configuration);

        services.AddRabbitMQ(configuration);

        services.AddTfsIntegration(configuration);

        services.AddTrelloIntegration(configuration);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
