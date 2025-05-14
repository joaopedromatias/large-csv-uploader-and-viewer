using Domain.Enums;
using Domain.Models;

namespace Domain.Factories;

public static class ExchangeFactory
{
    public static Exchange CreateExchange(string currencyCode, decimal value, int jobId) 
    {
        if (!Enum.IsDefined(typeof(CurrencyCode), currencyCode))
            throw new Exception("Invalid exchange");

        if (value <= 0)
            throw new Exception("Value must be a positive number");

        var exchange = new Exchange(Enum.Parse<CurrencyCode>(currencyCode), value, jobId);
        return exchange;
    }
}
