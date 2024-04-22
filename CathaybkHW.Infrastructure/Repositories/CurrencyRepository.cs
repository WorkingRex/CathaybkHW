using CathaybkHW.Domain.DomainServices.Currency.Interface;
using CathaybkHW.Domain.Entities.Currency;
using CathaybkHW.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

namespace CathaybkHW.Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly CathaybkHWDBContext _dbContext;

    public CurrencyRepository(CathaybkHWDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    async public Task<IEnumerable<CurrencyName>> GetCurrencyName()
    {
        var result = await _dbContext.CurrencyNames
            .ToArrayAsync();

        return result;
    }

    public async Task<CurrencyName> GetCurrencyNameByCodeAndLanguage(string code, string language)
    {
        var result = await _dbContext.CurrencyNames
            .FindAsync(code, language);

        return result;
    }

    public async Task AddCurrencyName(CurrencyName currencyName)
    {
        _dbContext.CurrencyNames.Add(currencyName);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCurrencyName(CurrencyName currencyName)
    {
        var entity = await GetCurrencyNameByCodeAndLanguage(currencyName.Code, currencyName.Language);
        if (entity != null)
        {
            entity.Name = currencyName.Name;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteCurrencyName(string code, string language)
    {
        var currencyName = await GetCurrencyNameByCodeAndLanguage(code, language);
        if (currencyName != null)
        {
            _dbContext.CurrencyNames.Remove(currencyName);
            await _dbContext.SaveChangesAsync();
        }
    }
}
