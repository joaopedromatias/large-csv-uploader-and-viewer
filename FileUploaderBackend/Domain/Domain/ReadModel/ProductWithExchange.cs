namespace Domain.ReadModel;

public class ProductWithExchange
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly Expiration { get; set; }
    public decimal PriceInUsd { get; set; }
    public decimal PriceInBrl { get; set; }
    public decimal PriceInEur { get; set; }
    public decimal PriceInGbp { get; set; }
    public decimal PriceInJpy { get; set; }
    public decimal PriceInArs { get; set; }
}

