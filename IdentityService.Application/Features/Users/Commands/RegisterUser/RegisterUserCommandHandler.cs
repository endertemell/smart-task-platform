using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using MediatR;

namespace IdentityService.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async  Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
       var existingUser = await _userRepository.GetUserByEmail(request.Email, cancellationToken);
       if (existingUser != null)
       {
           throw new Exception("User already exists");
       }
       
       //TODO Use bcrypt to hash the password
       var passwordHash = request.Password;
       var user = new User(request.FirstName, request.LastName, request.Email, passwordHash,Role.User);
       await _userRepository.AddUser(user, cancellationToken);
       return user.Id;
    }
}