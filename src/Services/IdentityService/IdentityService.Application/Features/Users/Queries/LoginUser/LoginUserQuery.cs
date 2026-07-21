using BuildingBlocks.Core;
using MediatR;

namespace IdentityService.Application.Features.Users.Queries.LoginUser;

public record LoginUserQuery(string Email, string Password) : IRequest<Result<LoginResponse>>
{
}