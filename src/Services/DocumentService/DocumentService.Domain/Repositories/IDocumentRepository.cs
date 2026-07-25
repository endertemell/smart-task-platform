using DocumentService.Domain.Entities;

namespace DocumentService.Domain.Repositories;

public interface IDocumentRepository
{
    Task Add(Document document, CancellationToken cancellationToken = default);
    Task<Document?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<List<Document>> GetByUserId(Guid userId, CancellationToken cancellationToken = default);
    Task Update(Document document, CancellationToken cancellationToken = default);
}