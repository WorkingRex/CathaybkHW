using CathaybkHW.Domain.DomainServices.Currency.Interface;
using CathaybkHW.Domain.DTOs.Currency;
using MediatR;

namespace CathaybkHW.Application.Services;

public class GetCurrencyNameQuery : IRequest<CurrencyName>
{
    public string Code { get; set; } = null!;
    public string Language { get; set; } = null!;
}

public class GetCurrencyNameQueryHandler : IRequestHandler<GetCurrencyNameQuery, CurrencyName>
{
    private readonly ICurrencyRepository _repository;

    public GetCurrencyNameQueryHandler(ICurrencyRepository repository)
    {
        _repository = repository;
    }

    public async Task<CurrencyName> Handle(GetCurrencyNameQuery request, CancellationToken cancellationToken)
    {
        var currencyName = await _repository.GetCurrencyNameByCodeAndLanguage(request.Code, request.Language);

        return new CurrencyName
        {
            Code = currencyName.Code,
            Language = currencyName.Language,
            Name = currencyName.Name
        };
    }
}