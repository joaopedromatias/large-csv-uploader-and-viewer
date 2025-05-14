using Domain.Models;

namespace Domain.Factories;

public static class ProductFactory
{
    public static Product CreateProduct(string name, decimal price, DateOnly expiration, int jobId)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new Exception("Product name must be provided");

        if (price < 0)
            throw new Exception($"Product price must not be negative: {price}");

        var product = new Product(name, price, expiration, jobId);
        return product;
    }
}
