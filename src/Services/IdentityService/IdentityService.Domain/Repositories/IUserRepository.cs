using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserById(Guid id,CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken = default);
    Task AddUser(User user, CancellationToken cancellationToken = default);
    Task Update(User user, CancellationToken cancellationToken = default);

}