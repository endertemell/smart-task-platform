namespace DocumentService.Application.Abstractions;

public interface IFileStorageService
{
    Task<string> SaveFile(Stream fileStream, string fileName,CancellationToken cancellationToken = default);
    Task DeleteFile(string filePath, CancellationToken cancellationToken = default);
}