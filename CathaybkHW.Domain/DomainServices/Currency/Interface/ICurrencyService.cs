using CathaybkHW.Domain.DTOs.Currency;

namespace CathaybkHW.Domain.DomainServices.Currency.Interface;

public interface ICurrencyService
{
    ExchangeRateResult GetExchangeRate(string fromCurrency, string toCurrency);
    CurrencyNameResult GetCurrencyName(string code);
}