using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application;

public static class DependecyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Bu katmandaki tüm MediatR Command ve Query'lerini otomatik bulup sisteme kaydeder
        services.AddMediatR(cfg=> cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
    
}