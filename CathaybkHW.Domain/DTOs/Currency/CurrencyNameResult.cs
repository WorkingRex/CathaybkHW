namespace CathaybkHW.Domain.DTOs.Currency;

public class CurrencyNameResult
{
    public string Code { get; set; }
    public List<CurrencyNameDetail> Names { get; set; }

    public CurrencyNameResult(string code, List<CurrencyNameDetail> names)
    {
        Code = code;
        Names = names;
    }
}

public class CurrencyNameDetail
{
    public string Language { get; set; }
    public string Name { get; set; }

    public CurrencyNameDetail(string language, string name)
    {
        Language = language;
        Name = name;
    }
}
