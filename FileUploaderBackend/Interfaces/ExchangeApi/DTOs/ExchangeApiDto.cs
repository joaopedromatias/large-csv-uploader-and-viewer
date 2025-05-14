namespace ExchangeApi.DTOs;

public record struct ExchangeApiDto
{
    public ExchangeData Usd {get; set;}
}

public record struct ExchangeData
{
    public decimal Brl {get;set;}
    public decimal Jpy {get;set;}
    public decimal Eur {get;set;}
    public decimal Gbp {get;set;}
    public decimal Ars {get;set;}
}