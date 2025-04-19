using System.Text;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Opulenza.Application.Authentication;
using Opulenza.Application.Behaviors;
using Opulenza.Application.ServiceContracts;
using Opulenza.Application.Services;
using Opulenza.Application.Settings;
using Scrutor;

namespace Opulenza.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<ApplicationMarker>();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining<ApplicationMarker>();

            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();


                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new { message = "Unauthorized" });
                        return context.Response.WriteAsync(result);
                    }
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGitHub(options =>
            {
                options.ClientId = configuration["GitHub:ClientId"];
                options.ClientSecret = configuration["GitHub:ClientSecret"];
                // Request additional scopes (if needed), e.g., to access user emails:
                options.Scope.Add("user:email");
                // This tells the GitHub provider to use the external sign-in cookie, which works well with Identity.
                options.SignInScheme = IdentityConstants.ExternalScheme;
            });

        services.AddAuthorization(options =>
        {
            // all action methods are authorized by default 
            // var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            // options.DefaultPolicy = policy;

            options.AddPolicy(AuthConstants.AdminUserPolicyName, policy =>
                policy.RequireClaim(AuthConstants.AdminUserClaimName, "true"));
            options.AddPolicy(AuthConstants.ApiKeyPolicyName, p => p.Requirements.Add(new ApiKeyRequirement()));
        });

        services.Scan(selector =>
        {
            selector.FromAssemblyOf<ApplicationMarker>()
                .AddClasses(f => f.InNamespaces("Opulenza.Application"))
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithScopedLifetime();
        });

        // services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddScoped<IAuthorizationHandler, ApiKeyAuthorizationHandler>();
        
        services.AddHttpContextAccessor();

        return services;
    }
}