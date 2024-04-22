using CathaybkHW.Domain.Entities.Currency;
using Microsoft.EntityFrameworkCore;

namespace CathaybkHW.Infrastructure.Databases;

public class CathaybkHWDBContext: DbContext
{
    public DbSet<CurrencyName> CurrencyNames { get; set; } = null!;

    public CathaybkHWDBContext(DbContextOptions<CathaybkHWDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyName>(entity =>
        {
            entity.HasKey(e => new { e.Code, e.Language });
        });
    }
}
