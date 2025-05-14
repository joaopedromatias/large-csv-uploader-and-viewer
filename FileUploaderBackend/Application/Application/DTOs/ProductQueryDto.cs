using Domain.Models;

namespace Application.DTOs;

public record struct ProductQueryDto
{
    private static readonly IList<string> VALID_ORDER_KEYS =
    [
        nameof(Product.Name).ToLower(), 
        nameof(Product.Expiration).ToLower(), 
        nameof(Product.Price).ToLower() 
    ];

    private static readonly int MAX_PAGE_SIZE = 500;

    public ProductQueryDto(string name, DateOnly expiration, uint page, uint pageSize, string orderKey, bool orderDesc = false)
    {
        Name = name;
        Expiration = expiration;
        Page = page;
        PageSize = pageSize;
        OrderKey = orderKey;
        OrderDesc = orderDesc;
    }

    public string? Name { get; set; }
    public DateOnly? Expiration { get; set; }
    public uint Page { get; set; }
    public uint PageSize { get; set; }
    public string OrderKey { get; set; }
    public bool OrderDesc { get; set; }

    public (bool IsValid, string Message) IsValid()
    {
        if (!VALID_ORDER_KEYS.Contains(OrderKey.ToLower()))
        {
            return (false, $"Invalid order key: {OrderKey}. The valids keys are {string.Join(", ", VALID_ORDER_KEYS)}");
        }

        if (Page < 0)
        {
            return (false, $"Page must not be negative: {Page}");
        }

        if (PageSize <= 0)
        {
            return (false, $"Page size must be positive: {PageSize}");
        }

        if (PageSize > MAX_PAGE_SIZE)
        {
            return (false, $"Page size must not be greater than {MAX_PAGE_SIZE}");
        }        

        return (true, string.Empty);
    }
}
