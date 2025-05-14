namespace Domain.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public DateOnly Expiration { get; private set; }
    public int JobId { get; private set; }

    public Product () { }

    internal Product(string name, decimal price, DateOnly expiration, int jobId)
    {
        Name = name;
        Price = price;
        Expiration = expiration;
        JobId = jobId;
    }
}