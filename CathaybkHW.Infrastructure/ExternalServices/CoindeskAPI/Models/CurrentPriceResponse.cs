using System.Text.Json.Serialization;

namespace CathaybkHW.Infrastructure.ExternalServices.CoindeskAPI.Models;

internal class CurrentPriceResponse
{
    public CurrentPriceTime Time { get; set; } = null!;
    public Dictionary<string, CurrentPriceRate> Bpi { get; set; } = null!;
}

internal class CurrentPriceTime
{
    public string UpdatedISO { get; set; } = null!;
}

internal class CurrentPriceRate
{
    public string Code { get; set; } = null!;

    [JsonPropertyName("rate_float")]
    public decimal RateFloat { get; set; }
}