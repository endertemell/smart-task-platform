using BuildingBlocks.Core;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Documents.Commands.UploadDocument;

public record UploadDocumentCommand(
    Guid UserId,
    string Title,
    IFormFile? File): IRequest<Result<Guid>>;
