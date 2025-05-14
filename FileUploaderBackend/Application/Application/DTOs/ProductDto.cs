using System.Globalization;

namespace Application.DTOs;

public record struct ProductFileDto
{
    public string name { get; set; } 
    public string price { get; set; }
    public string expiration { get; set; }

    public readonly string? Name
    {
        get 
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return name;
        }
    }

    public readonly decimal? Price
    {
        get 
        {
            if (string.IsNullOrWhiteSpace(price))
                return null;
            
            var isValid = decimal.TryParse(price.Split("$").Last(), out var decimalPrice);
            if (isValid)
                return decimalPrice;
            else 
                throw new Exception($"Invalid price: {price}");
        }
    }

    public readonly DateOnly? Expiration
    {
        get 
        {
            if (string.IsNullOrWhiteSpace(expiration))
                return null;

            if (DateOnly.TryParseExact(expiration, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return date;

            throw new FormatException($"Invalid expiration: {expiration}");
        }
    }    
}
