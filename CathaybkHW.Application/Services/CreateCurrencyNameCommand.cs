using CathaybkHW.Domain.DomainServices.Currency.Interface;
using CathaybkHW.Domain.DTOs.Currency;
using MediatR;

namespace CathaybkHW.Application.Services;

public class CreateCurrencyNameCommand : IRequest
{
    public CurrencyName CurrencyName { get; set; } = null!;
}

public class CreateCurrencyNameCommandHandler : IRequestHandler<CreateCurrencyNameCommand>
{
    private readonly ICurrencyRepository _repository;

    public CreateCurrencyNameCommandHandler(ICurrencyRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateCurrencyNameCommand request, CancellationToken cancellationToken)
    {
        await _repository.AddCurrencyName(new Domain.Entities.Currency.CurrencyName
        {
            Code = request.CurrencyName.Code,
            Language = request.CurrencyName.Language,
            Name = request.CurrencyName.Name
        });
    }
}