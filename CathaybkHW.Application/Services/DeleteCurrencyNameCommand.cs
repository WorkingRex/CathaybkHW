using CathaybkHW.Domain.DomainServices.Currency.Interface;
using MediatR;

namespace CathaybkHW.Application.Services;

public class DeleteCurrencyNameCommand : IRequest
{
    public string Code { get; set; } = null!;
    public string Language { get; set; } = null!;
}

public class DeleteCurrencyNameCommandHandler : IRequestHandler<DeleteCurrencyNameCommand>
{
    private readonly ICurrencyRepository _repository;

    public DeleteCurrencyNameCommandHandler(ICurrencyRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteCurrencyNameCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteCurrencyName(request.Code, request.Language);
        return;
    }
}