using CathaybkHW.Domain.DTOs.Currency;

namespace CathaybkHW.Domain.DomainServices.Currency;

public interface ICurrencyRepository
{
    CurrencyNameResult GetCurrencyName(string code);
}
