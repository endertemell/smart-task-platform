using IdentityService.Domain.Entities;

namespace IdentityService.Application.Abstractions;

public interface IJwtProvider
{
     public string GenerateToken(User user);
}