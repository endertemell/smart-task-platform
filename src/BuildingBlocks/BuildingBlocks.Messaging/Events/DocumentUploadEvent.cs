using BuildingBlocks.Messaging.Abstractions;

namespace BuildingBlocks.Messaging.Events;

public record DocumentUploadEvent(
    Guid DocumentId,
    Guid UserId,
    string Title,
    string FilePath,
    string ContentType) : IIntegrationEvent;
