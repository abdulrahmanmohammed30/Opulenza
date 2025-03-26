using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Opulenza.Domain.Categories;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Invoices;
using Opulenza.Domain.Entities.Orders;
using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Ratings;
using Opulenza.Domain.Entities.Roles;
using Opulenza.Domain.Entities.Shipments;
using Opulenza.Domain.Entities.Users;
using Opulenza.Domain.Entities.Wishlists;
using Opulenza.Domain.Payments;

namespace Opulenza.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options)
{
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
            
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}