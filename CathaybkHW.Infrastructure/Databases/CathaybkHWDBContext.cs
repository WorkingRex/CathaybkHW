using CathaybkHW.Domain.Entities.Currency;
using Microsoft.EntityFrameworkCore;

namespace CathaybkHW.Infrastructure.Databases;

public class CathaybkHWDBContext: DbContext
{
    public DbSet<Currency> Currencies { get; set; } = null!;
    public DbSet<CurrencyName> CurrencyNames { get; set; } = null!;

    public CathaybkHWDBContext(DbContextOptions<CathaybkHWDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Code);
        });

        modelBuilder.Entity<CurrencyName>(entity =>
        {
            entity.HasKey(e => new { e.Code, e.Language });

            entity.HasOne(e => e.Currency)
                .WithMany(e => e.Names)
                .HasForeignKey(e => e.Code)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
