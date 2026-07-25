using DocumentService.Domain.Enums;

namespace DocumentService.Domain.Entities;

public class Document
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string FileName { get; private set; } = string.Empty;
    public string FilePath { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long FileSize { get; private set; }
    public DocumentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Document() {}

    public Document(Guid userId, string title, string fileName, string filePath, string contentType, long fileSize)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        FileName = fileName;
        FilePath = filePath;
        ContentType = contentType;
        FileSize = fileSize;
        Status = DocumentStatus.Uploaded;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void MarkAsProcessing() => Status = DocumentStatus.Processing;
    public void MarkAsProcessed() => Status = DocumentStatus.Processed;
    public void MarkAsFailed() => Status = DocumentStatus.Failed;
}