using CathaybkHW.Infrastructure.ExternalServices.CoindeskAPI;
using Microsoft.Extensions.Configuration;
using Moq.Protected;
using Moq;
using System.Net.Http.Json;
using System.Net;
using CathaybkHW.Infrastructure.Enums;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace CathaybkHW.Infrastructure.Test.ExternalServices.CoindeskAPI;

public class ExchangeRateProviderTests
{
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private MockHttpMessageHandler _mockHttp;
    private HttpClient _client;
    private ExchangeRateProvider _provider;

    [SetUp]
    public void Setup()
    {
        _mockHttp = new MockHttpMessageHandler();
        _client = _mockHttp.ToHttpClient();
        _client.BaseAddress = new System.Uri("http://example.com");

        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _httpClientFactoryMock.Setup(x => x.CreateClient(nameof(HttpClientNames.CoindeskAPI))).Returns(_client);

        var appSettingsStub = new Dictionary<string, string> {
            {"CoindeskAPI:CurrentPrice", "currentprice.json"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub!)
            .Build();

        _provider = new ExchangeRateProvider(_httpClientFactoryMock.Object, configuration);
    }

    [TearDown]
    public void TearDown()
    {
        _mockHttp.Dispose();
        _client.Dispose();
    }

    [Test]
    public async Task GetRate_ReturnsCorrectData()
    {
        // Arrange
        string jsonResponse = @"
        {
            ""time"": {
                ""updatedISO"": ""2024-04-18T11:23:35+00:00""
            },
            ""bpi"": {
                ""USD"": {
                    ""code"": ""USD"",
                    ""rate_float"": 58921.6943
                },
                ""EUR"": {
                    ""code"": ""EUR"",
                    ""rate_float"": 49221.6943
                }
            }
        }";

        _mockHttp.When("http://example.com/currentprice.json")
                 .Respond("application/json", jsonResponse);  // Setup mock response

        var result = await _provider.GetRate();

        Assert.That(result.Count(), Is.EqualTo(2));
        var usdRate = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(usdRate.CurrencyCode, Is.EqualTo("USD"));
            Assert.That(usdRate.Rate, Is.EqualTo(58921.6943m));
        });
        var eurRate = result.Last();
        Assert.Multiple(() =>
        {
            Assert.That(eurRate.CurrencyCode, Is.EqualTo("EUR"));
            Assert.That(eurRate.Rate, Is.EqualTo(49221.6943m));
        });
    }

    [Test]
    public void GetRate_WhenApiResponseIsFailure_ThrowsHttpRequestException()
    {
        _mockHttp.When("http://example.com/currentprice.json")
                 .Respond(System.Net.HttpStatusCode.BadRequest); // Simulate an API failure

        Assert.ThrowsAsync<HttpRequestException>(async () => await _provider.GetRate());
    }

    [Test]
    public void GetRate_WhenApiResponseIsCorrupted_ThrowsJsonException()
    {
        string invalidJsonResponse = @"{""time"": {""updatedISO"": ""2024-04-18T11:23:35+00:00""}, ""bpi"": {""USD"": {""code"": ""USD"",""rate_float"": 58921.6943}, ""EUR"": {""code"": ""EUR""}}";
        _mockHttp.When("http://example.com/currentprice.json")
                 .Respond("application/json", invalidJsonResponse); // Simulate a corrupted JSON response

        Assert.ThrowsAsync<JsonException>(async () => await _provider.GetRate());
    }

    [Test]
    public void GetRate_WhenApiResponseIsSlow_ThrowsTimeoutException()
    {
        _mockHttp.When("http://example.com/currentprice.json")
                .Respond(async async =>
                {
                    await Task.Delay(TimeSpan.FromMicroseconds(10));
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("")
                    };
                });
        _client.Timeout = TimeSpan.FromMicroseconds(1);

        var ex = Assert.ThrowsAsync<TaskCanceledException>(async () => await _provider.GetRate());
        Assert.That(ex.InnerException, Is.TypeOf<TimeoutException>());
    }
}