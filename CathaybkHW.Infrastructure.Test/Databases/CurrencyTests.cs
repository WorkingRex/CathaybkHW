using CathaybkHW.Domain.Entities.Currency;
using CathaybkHW.Infrastructure.Databases;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CathaybkHW.Infrastructure.Test.Databases;

internal class CurrencyTests
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

        var currencyNames = new[]
        {
            new CurrencyName { Code = "USD", Language = "en-US", Name = "US Dollar" },
            new CurrencyName { Code = "USD", Language = "zh-TW", Name = "美元" },
            new CurrencyName { Code = "EUR", Language = "en-US", Name = "Euro" },
            new CurrencyName { Code = "EUR", Language = "zh-TW", Name = "歐元" },
            new CurrencyName { Code = "JPY", Language = "en-US", Name = "Japanese Yen" },
            new CurrencyName { Code = "JPY", Language = "zh-TW", Name = "日元" }
        };
        context.CurrencyNames.AddRange(currencyNames);

        context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        connection.Close();
    }
  

    [Test]
    public void Test_UpdateCurrencyName()
    {
        using var context = new CathaybkHWDBContext(dbContextOptions);
        var currencyName = context.CurrencyNames.FirstOrDefault(n => n.Code == "USD" && n.Language == "zh-TW");
        Assert.That(currencyName, Is.Not.Null);
        currencyName.Name = "美金";
        context.CurrencyNames.Update(currencyName);
        context.SaveChanges();

        var updatedName = context.CurrencyNames.FirstOrDefault(n => n.Code == "USD" && n.Language == "zh-TW");
        Assert.That(updatedName, Is.Not.Null);
        Assert.That(updatedName.Name, Is.EqualTo("美金"));
    }

    [Test]
    public async Task Test_AddNewCurrencyWithNamesAsync()
    {
        using var context = new CathaybkHWDBContext(dbContextOptions);

        var newNames = new[]
        {
            new CurrencyName { Code = "GBP", Language = "en-US", Name = "British Pound" },
            new CurrencyName { Code = "GBP", Language = "zh-TW", Name = "英鎊" }
        };
        context.CurrencyNames.AddRange(newNames);
        await context.SaveChangesAsync();

        var count = context.CurrencyNames.Count(n => n.Code == "GBP");
        Assert.That(count, Is.EqualTo(2));
    }

    [Test]
    public void Test_AddDuplicateCurrencyName()
    {
        using var context = new CathaybkHWDBContext(dbContextOptions);

        var ex = Assert.Throws<DbUpdateException>(() =>
        {
            context.CurrencyNames.Add(new CurrencyName { Code = "USD", Language = "en-US", Name = "" });
            context.SaveChanges();
        });

        Assert.That(ex.InnerException, Is.TypeOf<Microsoft.Data.Sqlite.SqliteException>());
    }
}
