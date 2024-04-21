namespace CathaybkHW.Domain.Entities.Currency;

public class CurrencyName
{
    public string Code { get; set; } = null!;
    public string Language { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Currency Currency { get; set; } = null!;
}
