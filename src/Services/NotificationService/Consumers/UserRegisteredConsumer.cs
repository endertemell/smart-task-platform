using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace NotificationService.Consumers;

public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly ILogger<UserRegisteredConsumer> _logger;

    public UserRegisteredConsumer(ILogger<UserRegisteredConsumer> logger)
    {
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("User registered: {UserId}, {Email}", message.UserId, message.Email);
        // send a notification, e.g., send an email or push notification to the user.
        await Task.CompletedTask;
    }
}