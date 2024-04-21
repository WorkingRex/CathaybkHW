using CathaybkHW.Domain.DTOs.Currency;

namespace CathaybkHW.Domain.DomainServices.Currency.Interface;

public interface ICurrencyRepository
{
    Task<IEnumerable<CurrencyNameResult>> GetCurrencyName();
}
