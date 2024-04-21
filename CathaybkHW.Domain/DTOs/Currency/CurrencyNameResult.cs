namespace CathaybkHW.Domain.DTOs.Currency;

public class CurrencyNameResult
{
    public string CurrencyCode { get; set; } = null!;
    public List<CurrencyNameDetail> Names { get; set; } = [];

}

public class CurrencyNameDetail
{
    public string Language { get; set; } = null!;
    public string Name { get; set; } = null!;

}
