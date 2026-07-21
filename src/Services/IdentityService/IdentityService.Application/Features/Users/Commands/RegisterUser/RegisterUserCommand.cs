using BuildingBlocks.Core;
using MediatR;

namespace IdentityService.Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Result<Guid>>;