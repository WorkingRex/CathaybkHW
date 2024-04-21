using CathaybkHW.Domain.DomainServices.Currency.Interface;
using CathaybkHW.Domain.DTOs.Currency;
using CathaybkHW.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

namespace CathaybkHW.Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly CathaybkHWDBContext _dBContext;

    public CurrencyRepository(CathaybkHWDBContext dBContext)
    {
        _dBContext = dBContext;
    }

    async public Task<IEnumerable<CurrencyNameResult>> GetCurrencyName()
    {
        var result = await _dBContext.Currencies
            .Include(c => c.Names)
            .Select(c => new CurrencyNameResult
            {
                CurrencyCode = c.Code,
                Names = c.Names.Select(n => new CurrencyNameDetail
                {
                    Language = n.Language,
                    Name = n.Name
                }).ToList()
            }).ToArrayAsync();

        return result;
    }
}
