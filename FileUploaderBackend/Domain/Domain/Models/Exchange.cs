using Domain.Enums;

namespace Domain.Models;

public class Exchange
{
    public int Id { get; set; }
    public CurrencyCode CurrencyCode {get; private set;}
    public decimal RateToUsd  {get; private set;}
    public int JobId {get; private set;}
    
    public Exchange () { }

    internal Exchange(CurrencyCode currencyCode, decimal value, int jobId)
    {
        CurrencyCode = currencyCode;
        RateToUsd = value;
        JobId = jobId;
    }
}
