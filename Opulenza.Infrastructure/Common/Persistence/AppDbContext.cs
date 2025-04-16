using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Invoices;
using Opulenza.Domain.Entities.Orders;
using Opulenza.Domain.Entities.Payments;
using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Ratings;
using Opulenza.Domain.Entities.Roles;
using Opulenza.Domain.Entities.Sessions;
using Opulenza.Domain.Entities.Shipments;
using Opulenza.Domain.Entities.Users;
using Opulenza.Domain.Entities.Wishlists;
using Opulenza.Infrastructure.Categories.Persistence;

namespace Opulenza.Infrastructure.Common.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options), IUnitOfWork
{
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<WishListItem> WishlistItems { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<CategoryRelationship> CategoryRelationships { get; set; }
    public DbSet<Session> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<CategoryRelationship>().HasNoKey().ToView(null);
        
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public async Task CommitChangesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
    
    public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            await action();
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}