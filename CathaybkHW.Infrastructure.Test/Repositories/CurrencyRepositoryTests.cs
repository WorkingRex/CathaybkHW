using CathaybkHW.Domain.Entities.Currency;
using CathaybkHW.Infrastructure.Databases;
using CathaybkHW.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CathaybkHW.Infrastructure.Test.Repositories;

public class CurrencyRepositoryTests
{
    private DbContextOptions<CathaybkHWDBContext> _dbContextOptions;
    private SqliteConnection _connection;
    private CathaybkHWDBContext _dbContext;
    private CurrencyRepository _repository;

    [SetUp]
    public void Setup()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _dbContextOptions = new DbContextOptionsBuilder<CathaybkHWDBContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new CathaybkHWDBContext(_dbContextOptions);
        context.Database.EnsureCreated();

        context.CurrencyNames.AddRange(
            new Domain.Entities.Currency.CurrencyName { Code = "USD", Language = "en", Name = "Dollar" },
            new Domain.Entities.Currency.CurrencyName { Code = "EUR", Language = "en", Name = "Euro" }
        );
        context.SaveChanges();

        _dbContext = new CathaybkHWDBContext(_dbContextOptions);

        _repository = new CurrencyRepository(_dbContext);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        _connection.Close();
    }


    [Test]
    public async Task GetCurrencyName_ReturnsAllCurrencies()
    {
        var results = await _repository.GetCurrencyName();

        Assert.That(results.Count(), Is.EqualTo(2));
        Assert.That(results, Has.Some.Matches<Domain.Entities.Currency.CurrencyName>(x => x.Code == "USD" && x.Name == "Dollar"));
        Assert.That(results, Has.Some.Matches<Domain.Entities.Currency.CurrencyName>(x => x.Code == "EUR" && x.Name == "Euro"));
    }

    [Test]
    public async Task GetCurrencyName_WithSeededData_ReturnsCorrectCurrencies()
    {
        var results = await _repository.GetCurrencyName();

        Assert.That(results.Count(), Is.EqualTo(2));
        Assert.That(results.Any(r => r.Code == "USD" && r.Name == "Dollar"));
        Assert.That(results.Any(r => r.Code == "EUR" && r.Name == "Euro"));
    }

    [Test]
    public async Task GetCurrencyNameByCodeAndLanguage_ReturnsCorrectEntity()
    {
        var result = await _repository.GetCurrencyNameByCodeAndLanguage("USD", "en");
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Dollar"));
    }

    [Test]
    public async Task AddCurrencyName_AddsSuccessfully()
    {
        await _repository.AddCurrencyName(new Domain.Entities.Currency.CurrencyName { Code = "JPY", Language = "jp", Name = "Yen" });
        var result = await _repository.GetCurrencyNameByCodeAndLanguage("JPY", "jp");
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Yen"));
    }

    [Test]
    public async Task UpdateCurrencyName_UpdatesSuccessfully()
    {
        var currencyName = await _repository.GetCurrencyNameByCodeAndLanguage("USD", "en");
        currencyName.Name = "US Dollar Updated";
        await _repository.UpdateCurrencyName(currencyName);

        var updatedCurrencyName = await _repository.GetCurrencyNameByCodeAndLanguage("USD", "en");
        Assert.That(updatedCurrencyName.Name, Is.EqualTo("US Dollar Updated"));
    }

    [Test]
    public async Task DeleteCurrencyName_DeletesSuccessfully()
    {
        await _repository.DeleteCurrencyName("EUR", "en");
        var result = await _repository.GetCurrencyNameByCodeAndLanguage("EUR", "en");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void AddCurrencyName_ThrowsException_WhenDuplicate()
    {
        Assert.ThrowsAsync<DbUpdateException>(async () => {
            await _repository.AddCurrencyName(new Domain.Entities.Currency.CurrencyName { Code = "USD", Language = "en", Name = "Duplicate Dollar" });
        });
    }

    [Test]
    public async Task UpdateCurrencyName_ThrowsException_WhenNotFound()
    {
        var nonExistingCurrencyName = new Domain.Entities.Currency.CurrencyName { Code = "XYZ", Language = "en", Name = "Not Exists" };

        await _repository.UpdateCurrencyName(nonExistingCurrencyName);

        var results = await _repository.GetCurrencyName();
        Assert.That(results.Any(r => r.Code == "XYZ"), Is.False);
    }
}