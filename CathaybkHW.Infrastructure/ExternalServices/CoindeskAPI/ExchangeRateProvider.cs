using CathaybkHW.Domain.DomainServices.Currency.Interface;
using CathaybkHW.Domain.DTOs.Currency;
using CathaybkHW.Infrastructure.Enums;
using CathaybkHW.Infrastructure.ExternalServices.CoindeskAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CathaybkHW.Infrastructure.ExternalServices.CoindeskAPI;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ExchangeRateProvider(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
    )
    {
        _httpClient = httpClientFactory.CreateClient(nameof(HttpClientNames.CoindeskAPI));
        _configuration = configuration;
    }

    async public Task<IEnumerable<ExchangeRate>> GetRate()
    {
        var response = await _httpClient.GetAsync(_configuration.GetValue<string>("CoindeskAPI:CurrentPrice")!);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        var responseData = JsonSerializer.Deserialize<CurrentPriceResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new Exception("Failed to deserialize response from Coindesk API");

        var lastUpdated = DateTime.Parse(responseData.Time.UpdatedISO);

        var result = responseData.Bpi.Select(x => new ExchangeRate
        {
            CurrencyCode = x.Value.Code,
            Rate = x.Value.RateFloat,
            LastUpdated = lastUpdated
        }).ToArray();

        return result;
    }
}
