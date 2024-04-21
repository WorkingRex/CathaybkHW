using CathaybkHW.Domain.DTOs.Currency;

namespace CathaybkHW.Domain.DomainServices.Currency;

public interface IExchangeRateProvider
{
    ExchangeRateResult GetRate(string code);
}
