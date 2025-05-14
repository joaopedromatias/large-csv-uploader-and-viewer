using Application.DTOs;
using Domain.Models;
using Domain.ReadModel;

namespace Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductWithExchange>> GetProductsWithExchange(ProductQueryDto query, CancellationToken cancellationToken);
}
