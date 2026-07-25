using BuildingBlocks.Core;
using BuildingBlocks.Messaging.Abstractions;
using BuildingBlocks.Messaging.Events;
using DocumentService.Application.Abstractions;
using DocumentService.Domain.Repositories;
using MediatR;
using Document = DocumentService.Domain.Entities.Document;

namespace DocumentService.Application.Features.Documents.Commands.UploadDocument;

public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand,Result<Guid>>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IEventBus _eventBus;

    public UploadDocumentCommandHandler(IDocumentRepository documentRepository,IFileStorageService fileStorageService,
        IEventBus eventBus)
    {
        _documentRepository = documentRepository;
        _fileStorageService = fileStorageService;
        _eventBus = eventBus;
    }
    public async Task<Result<Guid>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        if (request.File  ==  null || request.File.Length == 0)
        {
            return Result<Guid>.Failure("File is empty");
        }

        string? savedFilePath = null;
        try
        {

            await using var stream = request.File.OpenReadStream();
            var filePath = await _fileStorageService.SaveFile(stream, request.File.FileName, cancellationToken);

            var document = new Document(
                request.UserId,
                request.Title,
                request.File.FileName,
                filePath,
                request.File.ContentType,
                request.File.Length
            );

            await _documentRepository.Add(document, cancellationToken);
            await _eventBus.PublishAsync(new DocumentUploadEvent(
                document.Id,
                document.UserId,
                document.Title,
                document.FilePath,
                document.ContentType), cancellationToken);

            return Result<Guid>.Success(document.Id);
        }
        
        catch (Exception ex)
        {
            if (!string.IsNullOrEmpty(savedFilePath))
            {
                await _fileStorageService.DeleteFile(savedFilePath, cancellationToken);
            }
            return Result<Guid>.Failure(ex.Message);
        }
    }
}