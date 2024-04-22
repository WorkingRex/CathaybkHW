using CathaybkHW.Domain.Entities.Currency;

namespace CathaybkHW.Domain.DomainServices.Currency.Interface;

public interface ICurrencyRepository
{
    Task<IEnumerable<CurrencyName>> GetCurrencyName();

    Task<CurrencyName> GetCurrencyNameByCodeAndLanguage(string code, string language);

    Task AddCurrencyName(CurrencyName currencyName);

    Task UpdateCurrencyName(CurrencyName currencyName);

    Task DeleteCurrencyName(string code, string language);
}
