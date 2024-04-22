using CathaybkHW.Domain.DTOs.Currency;

namespace CathaybkHW.Domain.DomainServices.Currency.Interface;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetRate();
}
