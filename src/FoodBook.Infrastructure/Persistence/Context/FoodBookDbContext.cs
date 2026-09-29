using Microsoft.EntityFrameworkCore;

namespace FoodBook.Infrastructure.Persistence.Context;

public class FoodBookDbContext : DbContext
{
    public FoodBookDbContext(DbContextOptions<FoodBookDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply every IEntityTypeConfiguration<T> found in this assembly.
        // Configurations/ is where you will put them.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FoodBookDbContext).Assembly);
    }
}
