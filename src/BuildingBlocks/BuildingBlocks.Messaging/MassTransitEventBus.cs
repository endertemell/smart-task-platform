using BuildingBlocks.Messaging.Abstractions;
using BuildingBlocks.Messaging.Enums;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging;

public class MassTransitEventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public MassTransitEventBus(IServiceProvider serviceProvider,IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IIntegrationEvent
    {
        if (Enum.TryParse<MessageBusType>(_configuration["MessageBus:Provider"], true, out var providerType))
        {
            switch (providerType)
            {
                case MessageBusType.RabbitMq:
                    var publishEndpoint = _serviceProvider.GetRequiredService<IPublishEndpoint>();
                    await publishEndpoint.Publish(@event, cancellationToken);
                    break;
                case MessageBusType.Kafka:
                    var topicProducer = _serviceProvider.GetRequiredService<ITopicProducer<T>>();
                    await topicProducer.Produce(@event, cancellationToken);
                    break;
                default:
                    throw new NotSupportedException($"Publishing via {providerType} is not supported.");
            }
        }
      
    }
}