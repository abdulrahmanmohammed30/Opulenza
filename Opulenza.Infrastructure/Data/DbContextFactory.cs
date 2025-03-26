using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Opulenza.Infrastructure.Data;

public class DbContextFactory: IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=Opulenza; Integrated Security = true; Trust Server Certificate = true");
        return new AppDbContext(optionsBuilder.Options);
    }
}