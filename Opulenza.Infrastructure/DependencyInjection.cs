using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Roles;
using Opulenza.Domain.Entities.Users;
using Opulenza.Infrastructure.Carts.Persistence;
using Opulenza.Infrastructure.Common.Persistence;
using Opulenza.Infrastructure.Interceptors;
using Opulenza.Infrastructure.Settings;
using Opulenza.Infrastructure.Users.Persistence;
using Opulenza.Infrastructure.Wishlists.Persistence;

namespace Opulenza.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).AddInterceptors(new SoftDeleteInterceptor())
                .LogTo(Console.WriteLine, LogLevel.Critical)
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .UseSeeding( (context, _) =>
                {
                    if ((context.Set<ApplicationRole>().Any()) == false)
                    {
                        var roles = new List<ApplicationRole>()
                        {
                            new ApplicationRole() { Name = "User", NormalizedName = "USER" },
                            new ApplicationRole() { Name = "Admin", NormalizedName = "ADMIN" },
                        };

                        context.Set<ApplicationRole>().AddRange(roles);
                        context.SaveChanges();
                    }
                }).UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    if ((await context.Set<ApplicationRole>().AnyAsync()) == false)
                    {
                        var roles = new List<ApplicationRole>()
                        {
                            new ApplicationRole() { Name = "User" },
                            new ApplicationRole() { Name = "Admin" },
                        };

                        context.Set<ApplicationRole>().AddRange(roles);
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IWishlistRepository, WishlistRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());
        services.AddScoped(typeof(ISoftDeleteRepository<>), typeof(SoftDeleteRepository<>));
        services.AddScoped(typeof(IOptionalSoftDeleteRepository<>), typeof(OptionalSoftDeleteRepository<>));
        services.AddScoped<IUserAddressRepository, UserAddressRepository>();
        
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                // options.Lockout.MaxFailedAccessAttempts = 5;
                // options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, int>>()
            .AddRoleStore<RoleStore<ApplicationRole, AppDbContext, int>>();

        services.AddScoped<IUserImageRepository, UserImageRepository>();
        
        services.Configure<FileSettings>(configuration.GetSection("FileSettings"));
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

        return services;
    }
}