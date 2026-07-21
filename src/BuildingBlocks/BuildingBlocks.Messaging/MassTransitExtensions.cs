using System.Reflection;
using BuildingBlocks.Messaging.Abstractions;
using BuildingBlocks.Messaging.Enums;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging;

public static class MassTransitExtensions
{
    public static IServiceCollection AddCustomMassTransit(this IServiceCollection services,
        IConfiguration configuration, Assembly assembly)
    {
        services.AddScoped<IEventBus, MassTransitEventBus>();

        if (Enum.TryParse<MessageBusType>(configuration["MessageBus:Provider"], true, out var providerType))
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(assembly);

                switch (providerType)
                {
                    case MessageBusType.RabbitMq:
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            var host = configuration["MessageBus:RabbitMq:Host"];
                            var username = configuration["MessageBus:RabbitMq:Username"];
                            var password = configuration["MessageBus:RabbitMq:Password"];
                            cfg.Host(host, "/", h =>
                            {
                                if (username != null) h.Username(username);
                                if (password != null) h.Password(password);
                            });
                            cfg.ConfigureEndpoints(context);
                        });
                        break;
                    case MessageBusType.Kafka:
                        x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

                        x.AddRider(rider =>
                        {
                            var eventTypes = typeof(IIntegrationEvent).Assembly.GetTypes()
                                .Where(t => typeof(IIntegrationEvent).IsAssignableFrom(t) &&
                                            t is { IsClass: true, IsAbstract: false });

                            var addProducerMethod = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(a => a.GetTypes())
                                .Where(t => t is
                                    { IsClass: true, IsSealed: true, IsAbstract: true }) 
                                .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                                .FirstOrDefault(m => m.Name == "AddProducer" &&
                                                     m.GetParameters().Length == 2 &&
                                                     m.GetParameters()[0].ParameterType.Name
                                                         .Contains("IRiderRegistrationConfigurator") &&
                                                     m.GetParameters()[1].ParameterType == typeof(string));

                            if (addProducerMethod == null)
                            {
                                throw new Exception("MassTransit AddProducer metodu bulunamadı!");
                            }

                            foreach (var eventType in eventTypes)
                            {
                                var topicName = KebabCaseEndpointNameFormatter.Instance.SanitizeName(eventType.Name);
                                var genericMethod = addProducerMethod.MakeGenericMethod(eventType);
                                genericMethod.Invoke(null, new object[] { rider, topicName });
                            }

                            rider.UsingKafka((context, k) =>
                            {
                                var host = configuration["MessageBus:Kafka:Host"];
                                k.Host(host);
                            });
                        });

                        break;
                    default:
                        throw new ArgumentException("Invalid MessageBus provider type in configuration.");
                }
            });
        }

        return services;
    }
}