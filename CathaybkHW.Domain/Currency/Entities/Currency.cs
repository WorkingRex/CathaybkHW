namespace CathaybkHW.Domain.Currency.Entities;

public class Currency
{
    public string Code { get; set; } = null!;
    public ICollection<CurrencyName> Names { get; set; } = [];
}
