using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;
using IdentityService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        // IUserRepository istendiğinde UserRepository çalıştırılsın (Scoped = Her HTTP isteğinde yeni bir tane yaratılır)
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}