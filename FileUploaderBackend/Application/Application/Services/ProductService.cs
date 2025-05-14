using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.ReadModel;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductWithExchange>> GetProductsWithExchange(ProductQueryDto query, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsWithExchange
            (query.Name, query.Expiration, query.Page, query.PageSize, query.OrderKey, query.OrderDesc, cancellationToken);
        return products;
    }
}
