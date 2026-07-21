using BuildingBlocks.Core;
using IdentityService.Application.Abstractions;
using IdentityService.Domain.Repositories;
using MediatR;

namespace IdentityService.Application.Features.Users.Queries.LoginUser;

public class LoginUserQueryHandler: IRequestHandler<LoginUserQuery, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserQueryHandler(IUserRepository userRepository,IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<Result<LoginResponse>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email, cancellationToken);
        if(user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Result<LoginResponse>.Failure("Invalid email or password");
        }

        var token = _jwtProvider.GenerateToken(user);
        return Result<LoginResponse>.Success(new LoginResponse(token));
    }
}