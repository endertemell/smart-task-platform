using BuildingBlocks.Messaging.Abstractions;
using MassTransit;

namespace BuildingBlocks.Messaging;

public class MassTransitEventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent
    {
        await _publishEndpoint.Publish(@event, cancellationToken);
    }
}