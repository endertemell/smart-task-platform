using DocumentService.Domain.Entities;
using DocumentService.Domain.Repositories;
using DocumentService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DocumentService.Infrastructure.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly IMongoCollection<Document> _documentsCollection;

    public DocumentRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings)
    {
        _documentsCollection = database.GetCollection<Document>(settings.Value.DocumentsCollectionName);
    }

    public async Task Add(Document document, CancellationToken cancellationToken = default)
    {
        await _documentsCollection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task<Document?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _documentsCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Document>> GetByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _documentsCollection.Find(x => x.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task Update(Document document, CancellationToken cancellationToken = default)
    {
        await _documentsCollection.ReplaceOneAsync(x => x.Id == document.Id, document, cancellationToken: cancellationToken);
    }
}