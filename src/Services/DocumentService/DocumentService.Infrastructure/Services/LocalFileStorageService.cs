using DocumentService.Application.Abstractions;

namespace DocumentService.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _uploadDirectory;
    
    public LocalFileStorageService()
    {
        _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        if (!Directory.Exists(_uploadDirectory))
        {
            Directory.CreateDirectory(_uploadDirectory);
        }
        
    }
    public async Task<string> SaveFile(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(_uploadDirectory, uniqueFileName);
        using (var destinationStream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(destinationStream, cancellationToken);
        }
        return filePath;
    }

    public Task DeleteFile(string filePath, CancellationToken cancellationToken = default)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        return Task.CompletedTask;
    }
}