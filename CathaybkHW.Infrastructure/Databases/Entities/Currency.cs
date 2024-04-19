namespace CathaybkHW.Infrastructure.Databases.Entities;

public class Currency
{
    public string Code { get; set; } = null!;
    public ICollection<CurrencyName> Names { get; set; } = [];
}
