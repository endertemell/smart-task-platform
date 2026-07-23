using BuildingBlocks.Messaging.Abstractions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging;

public static class MassTransitExtensions
{
    public static IServiceCollection AddCustomMassTransit(this IServiceCollection services,
        IConfiguration configuration, Assembly assembly)
    {
        services.AddScoped<IEventBus, MassTransitEventBus>();

        services.AddMassTransit(x =>
        {
            x.AddConsumers(assembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration["MessageBus:RabbitMq:Host"] ?? "localhost";
                var username = configuration["MessageBus:RabbitMq:Username"] ?? "guest";
                var password = configuration["MessageBus:RabbitMq:Password"] ?? "guest";

                cfg.Host(host, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}