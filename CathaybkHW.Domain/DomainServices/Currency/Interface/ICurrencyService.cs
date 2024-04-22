using CathaybkHW.Domain.DTOs.Currency;

namespace CathaybkHW.Domain.DomainServices.Currency.Interface;

public interface ICurrencyService
{
    ExchangeRate GetExchangeRate(string fromCurrency, string toCurrency);
    CurrencyName GetCurrencyName(string code);
}