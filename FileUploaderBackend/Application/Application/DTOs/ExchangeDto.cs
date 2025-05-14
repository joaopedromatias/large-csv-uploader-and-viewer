namespace Application.DTOs;

public record struct ExchangeDto
{
    public string CurrencyCode { get; set; }
    public decimal RateToUsd { get; set; }
}
