namespace CathaybkHW.Application.Result;

public class CurrencyInfoResult
{

    public string Code { get; set; } = null!;

    public decimal ExchangeRate { get; set; }

    public string UpdatedTime { get; set; } = null!;

    public IEnumerable<CurrencyNameResult> Names { get; set; } = null!;
}

public class CurrencyNameResult
{
    public string Language { get; set; } = null!;
    public string Name { get; set; } = null!;
}