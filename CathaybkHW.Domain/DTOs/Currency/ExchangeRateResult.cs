namespace CathaybkHW.Domain.DTOs.Currency;

public class ExchangeRateResult
{
    public decimal Rate { get; set; }
    public DateTime LastUpdated { get; set; }

    public ExchangeRateResult(decimal rate, DateTime lastUpdated)
    {
        Rate = rate;
        LastUpdated = lastUpdated;
    }
}
