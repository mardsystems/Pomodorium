using Microsoft.Extensions.DependencyInjection;
using Pomodorium.Logging;
using System.Reflection;

namespace Pomodorium.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
