using BuildingBlocks.Core;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected ActionResult<AppResponse<T>> HandleResult<T>(Result<T> result, string successMessage = "")
    {
        if (result.IsSuccess)
        {
            return Ok(AppResponse<T>.Success(result.Value, successMessage));
        }

        return BadRequest(AppResponse<T>.Failure(result.Errors));
    }
}
