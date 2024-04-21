using CathaybkHW.Application.Result;
using CathaybkHW.Domain.DomainServices.Currency.Interface;
using MediatR;

namespace CathaybkHW.Application.Services;

public class CurrencyInfoListQuery : IRequest<IEnumerable<CurrencyInfoResult>>
{
}


public class CurrencyInfoListQueryHandler : IRequestHandler<CurrencyInfoListQuery, IEnumerable<CurrencyInfoResult>>
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public CurrencyInfoListQueryHandler(ICurrencyRepository currencyRepository, IExchangeRateProvider exchangeRateProvider)
    {
        _currencyRepository = currencyRepository;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<IEnumerable<CurrencyInfoResult>> Handle(CurrencyInfoListQuery request, CancellationToken cancellationToken)
    {
        var currencyRates = await _exchangeRateProvider.GetRate();
        var currencyNames = await _currencyRepository.GetCurrencyName();

        var result = (
            from rate in currencyRates
            join name in currencyNames on rate.CurrencyCode equals name.CurrencyCode into temps
            from name in temps.DefaultIfEmpty()
            select new CurrencyInfoResult
            {
                Code = rate.CurrencyCode,
                ExchangeRate = rate.Rate,
                UpdatedTime = rate.LastUpdated.ToString("yyyy/MM/dd HH:mm:ss"),
                Names = name?.Names.Select(x => new CurrencyNameResult
                {
                    Language = x.Language,
                    Name = x.Name
                }) ?? []
            });

        return result;
    }
}