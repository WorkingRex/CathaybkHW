using CathaybkHW.Domain.DomainServices.Currency.Interface;
using CathaybkHW.Domain.DTOs.Currency;
using MediatR;

namespace CathaybkHW.Application.Services;

public class UpdateCurrencyNameCommand : IRequest
{
    public CurrencyName CurrencyName { get; set; } = null!;
}

public class UpdateCurrencyNameCommandHandler : IRequestHandler<UpdateCurrencyNameCommand>
{
    private readonly ICurrencyRepository _repository;

    public UpdateCurrencyNameCommandHandler(ICurrencyRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateCurrencyNameCommand request, CancellationToken cancellationToken)
    {
        await _repository.UpdateCurrencyName(new Domain.Entities.Currency.CurrencyName
        {
            Code = request.CurrencyName.Code,
            Language = request.CurrencyName.Language,
            Name = request.CurrencyName.Name
        });
    }
}
