﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FlowtimeTechnique.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFlowtimeTechnique(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}