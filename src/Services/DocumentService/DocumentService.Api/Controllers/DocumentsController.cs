using BuildingBlocks.Core;
using BuildingBlocks.Core.Controllers;
using DocumentService.Application.Features.Documents.Commands.UploadDocument;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DocumentsController : BaseController
{
    private readonly ISender _sender;

    public DocumentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AppResponse<Guid>>> UploadDocument(
        [FromForm] string title,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var command = new UploadDocumentCommand(CurrentUserId, title, file);
        var result = await _sender.Send(command, cancellationToken);

        return HandleResult(result, "Document uploaded successfully");
    }
}
