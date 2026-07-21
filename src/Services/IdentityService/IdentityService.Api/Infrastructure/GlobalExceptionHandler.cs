using BuildingBlocks.Core;
using Microsoft.AspNetCore.Diagnostics;

namespace IdentityService.Api.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        // Prepare the standard AppResponse format to return to the user
        var response = AppResponse<object>.Failure(exception.Message, "An error occurred while processing your request.");

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true; 
    }
}
