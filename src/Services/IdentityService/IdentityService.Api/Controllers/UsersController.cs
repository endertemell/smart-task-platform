using BuildingBlocks.Core;
using IdentityService.Application.Features.Users.Commands.RegisterUser;
using IdentityService.Application.Features.Users.Queries.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AppResponse<Guid>>> Register([FromBody] RegisterUserCommand command)
    {
        return HandleResult(await _mediator.Send(command), "User registered successfully");
    }

    [HttpPost("login")]
    public async Task<ActionResult<AppResponse<LoginResponse>>> Login([FromBody] LoginUserQuery command)
    {
        return HandleResult(await _mediator.Send(command), "User logged in successfully");
    }
}