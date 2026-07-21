using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using MediatR;

using BuildingBlocks.Core;
using BuildingBlocks.Messaging.Abstractions;
using BuildingBlocks.Messaging.Events;

namespace IdentityService.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventBus _eventBus;

    public RegisterUserCommandHandler(IUserRepository userRepository,IEventBus eventBus)
    {
        _userRepository = userRepository;
        _eventBus = eventBus;
    }
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
       var existingUser = await _userRepository.GetUserByEmail(request.Email, cancellationToken);
       if (existingUser != null)
       {
           return Result<Guid>.Failure("User already exists");
       }
       
       var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
       var user = new User(request.FirstName, request.LastName, request.Email, passwordHash,Role.User);
       await _userRepository.AddUser(user, cancellationToken);
       await _eventBus.PublishAsync(new UserRegisteredEvent(user.Id, user.FirstName, user.LastName, user.Email), cancellationToken);
       return Result<Guid>.Success(user.Id);
    }
}