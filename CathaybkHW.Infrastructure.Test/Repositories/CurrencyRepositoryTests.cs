using CathaybkHW.Domain.DTOs.Currency;
using CathaybkHW.Domain.Entities.Currency;
using CathaybkHW.Infrastructure.Databases;
using CathaybkHW.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CathaybkHW.Infrastructure.Test.Repositories;

public class CurrencyRepositoryTests
{
    private DbContextOptions<CathaybkHWDBContext> dbContextOptions;
    private SqliteConnection connection;

    [SetUp]
    public void Setup()
    {
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        dbContextOptions = new DbContextOptionsBuilder<CathaybkHWDBContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new CathaybkHWDBContext(dbContextOptions);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Seed the database
        context.Currencies.Add(new Currency
        {
            Code = "USD",
            Names = new List<CurrencyName>
            {
                new CurrencyName { Language = "en", Name = "Dollar" }
            }
        });
        context.Currencies.Add(new Currency
        {
            Code = "EUR",
            Names = new List<CurrencyName>
            {
                new CurrencyName { Language = "en", Name = "Euro" }
            }
        });
        context.SaveChanges();
    }

    [TearDown]
    public void Cleanup()
    {
        connection.Close();
    }

    [Test]
    public async Task GetCurrencyName_ReturnsAllCurrencies()
    {
        using var context = new CathaybkHWDBContext(dbContextOptions);
        var repository = new CurrencyRepository(context);

        var results = await repository.GetCurrencyName();

        Assert.That(results.Count(), Is.EqualTo(2));
        Assert.That(results, Has.Some.Matches<CurrencyNameResult>(x => x.CurrencyCode == "USD" && x.Names.Any(n => n.Name == "Dollar")));
        Assert.That(results, Has.Some.Matches<CurrencyNameResult>(x => x.CurrencyCode == "EUR" && x.Names.Any(n => n.Name == "Euro")));
    }

    [Test]
    public async Task GetCurrencyName_WithSeededData_ReturnsCorrectCurrencies()
    {
        using var dbContext = new CathaybkHWDBContext(dbContextOptions);
        var repository = new CurrencyRepository(dbContext);

        var results = await repository.GetCurrencyName();

        Assert.That(results.Count(), Is.EqualTo(2));
        Assert.That(results.Any(r => r.CurrencyCode == "USD" && r.Names.Any(n => n.Name == "Dollar")));
        Assert.That(results.Any(r => r.CurrencyCode == "EUR" && r.Names.Any(n => n.Name == "Euro")));
    }
}