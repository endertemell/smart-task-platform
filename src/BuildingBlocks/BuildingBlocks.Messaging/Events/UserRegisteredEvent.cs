using BuildingBlocks.Messaging.Abstractions;

namespace BuildingBlocks.Messaging.Events;


public record UserRegisteredEvent(Guid UserId, string FirstName, string LastName, string Email) : IIntegrationEvent;
