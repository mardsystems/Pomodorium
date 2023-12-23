using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Pomodorium.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationRemote(this IServiceCollection services)
    {
        services.AddScoped<FlowTimerClient>();
        services.AddScoped<PomodoroTimerClient>();
        services.AddScoped<TaskManagerClient>();
        services.AddScoped<TaskSynchronizerClient>();
        services.AddScoped<MaintenanceClient>();
        services.AddScoped<WeatherForecastClient>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
