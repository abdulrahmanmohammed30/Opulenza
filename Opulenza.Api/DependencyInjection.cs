using System.Collections.Immutable;
using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Opulenza.Api.Authentication;
using Opulenza.Api.Services;
using Opulenza.Application;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Common.Utilities;
using Opulenza.Infrastructure;
using Serilog;

namespace Opulenza.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options => { options.Conventions.Add(new HttpVerbConvention()); });
        services.AddCors(options =>
        {
            options.AddPolicy("default", p => p.AllowAnyOrigin()
                .AllowAnyMethod().AllowAnyHeader());
        });

        services.AddResponseCaching(x => { x.SizeLimit = 1024; });

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

            setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });
            
            // Apply the security scheme globally
            setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // setupAction.AddSecurityDefinition("Apikey", new OpenApiSecurityScheme
            // {
            //     Description = "The API Key to access the API",
            //     Name = "x-api-key",
            //     In = ParameterLocation.Header,
            //     Type = SecuritySchemeType.ApiKey,
            //     Scheme = "ApiKeyScheme"
            // });
            //
            // var scheme = new OpenApiSecurityScheme()
            // {
            //     Reference = new OpenApiReference()
            //     {
            //         Type = ReferenceType.SecurityScheme,
            //         Id = "ApiKey"
            //     },
            //     In = ParameterLocation.Header
            // };
            //
            // var requirement = new OpenApiSecurityRequirement()
            // {
            //     { scheme, new List<string>() }
            // };
            //
            // setupAction.AddSecurityRequirement(requirement);
        });


        Serilog.ILogger logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(services.BuildServiceProvider())
            .CreateLogger();

        Log.Logger = logger;

        services.AddSingleton(logger);
        services.AddScoped<ApiKeyAuthFilter>();
        return services;
    }
}