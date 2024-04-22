namespace CathaybkHW.Domain.DTOs.Currency;

public class ExchangeRate
{
    public string CurrencyCode { get; set; } = null!;
    public decimal Rate { get; set; }
    public DateTime LastUpdated { get; set; }
}
