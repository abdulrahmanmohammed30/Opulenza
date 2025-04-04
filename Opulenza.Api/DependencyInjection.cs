using Opulenza.Api.Services;
using Opulenza.Application;
using Opulenza.Application.Common.interfaces;
using Opulenza.Infrastructure;

namespace Opulenza.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration) {
        services.AddControllers();
        services.AddCors(options =>
        {
            options.AddPolicy("default", p => p.AllowAnyOrigin()
                .AllowAnyMethod().AllowAnyHeader());
        });
        
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        services.AddInfrastructure(configuration);
        services.AddApplication(configuration);
        
        return services;
    }
}