using System.Collections.Immutable;
using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Opulenza.Api.Services;
using Opulenza.Application;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Common.Utilities;
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
        services.AddScoped<IUrlGenerator, UrlGenerator>();
        services.AddInfrastructure(configuration);
        services.AddApplication(configuration);

        services.AddApiVersioning(setupAction =>
        {
            setupAction.DefaultApiVersion = new ApiVersion(1.0);
            setupAction.AssumeDefaultVersionWhenUnspecified = true;
            setupAction.ReportApiVersions = true;
            setupAction.ApiVersionReader = new HeaderApiVersionReader("X-Api-Version");

        }).AddMvc().AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(setupAction =>
        {
            setupAction.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1", 
                Title = "Opulenza API",
            });
            
        });
        
        
        return services;
    }
}