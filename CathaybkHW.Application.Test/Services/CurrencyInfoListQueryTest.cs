using CathaybkHW.Application.Services;
using CathaybkHW.Domain.DomainServices.Currency.Interface;
using Moq;

using DTOs = CathaybkHW.Domain.DTOs.Currency;
using Entities = CathaybkHW.Domain.Entities.Currency;

namespace CathaybkHW.Application.Test.Services;

internal class CurrencyInfoListQueryHandlerTests
{
    private Mock<ICurrencyRepository> _currencyRepositoryMock;
    private Mock<IExchangeRateProvider> _exchangeRateProviderMock;
    private CurrencyInfoListQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        _exchangeRateProviderMock = new Mock<IExchangeRateProvider>();

        _currencyRepositoryMock.Setup(x => x.GetCurrencyName())
            .ReturnsAsync(new List<Entities.CurrencyName>
            {
                new() {
                    Code = "USD",
                    Language = "en",
                    Name = "Dollar"
                },
                new() {
                    Code = "EUR",
                    Language = "en",
                    Name = "Euro"
                }
            });

        _exchangeRateProviderMock.Setup(x => x.GetRate())
            .ReturnsAsync(new List<DTOs.ExchangeRate>
            {
                new() { CurrencyCode = "USD", Rate = 30.1m, LastUpdated = DateTime.Now },
            });

        _handler = new CurrencyInfoListQueryHandler(_currencyRepositoryMock.Object, _exchangeRateProviderMock.Object);
    }

    [Test]
    public void Handle_WhenCurrencyRepositoryThrowsException_ThrowsException()
    {
        _currencyRepositoryMock.Setup(x => x.GetCurrencyName())
            .ThrowsAsync(new Exception("Failed to fetch currency names"));

        Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(new CurrencyInfoListQuery(), CancellationToken.None));
    }

    [Test]
    public void Handle_WhenExchangeRateProviderThrowsException_ThrowsException()
    {
        _exchangeRateProviderMock.Setup(x => x.GetRate())
            .ThrowsAsync(new Exception("Failed to fetch exchange rates"));

        Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(new CurrencyInfoListQuery(), CancellationToken.None));
    }

    [Test]
    public async Task Handle_WhenDataMisalignmentOccurs_HandlesGracefully()
    {
        var results = await _handler.Handle(new CurrencyInfoListQuery(), CancellationToken.None);

        Assert.That(results.Count(), Is.EqualTo(1));
    }
}