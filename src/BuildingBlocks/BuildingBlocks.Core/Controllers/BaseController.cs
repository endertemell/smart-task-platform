using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Core.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected Guid CurrentUserId
    {
        get
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated or the user ID claim is missing.");
            }

            return userId;
        }
    }

    protected ActionResult<AppResponse<T>> HandleResult<T>(Result<T> result, string successMessage = "")
    {
        if (result.IsSuccess)
        {
            return Ok(AppResponse<T>.Success(result.Value, successMessage));
        }

        return BadRequest(AppResponse<T>.Failure(result.Errors));
    }
}
