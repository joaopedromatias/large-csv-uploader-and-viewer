using Domain.Models;
using Domain.ReadModel;

namespace Domain.Interfaces.Repositories;

public interface IProductRepository
{
    Task<IList<ProductWithExchange>> GetProductsWithExchange(string? name, DateOnly? expiration, uint page, uint pageSize, string orderBy, bool descendingOrder, CancellationToken cancellationToken);
    Task CreateBatch(IList<Product> products);
    Task DeleteAllFromJob(int jobId);
}
