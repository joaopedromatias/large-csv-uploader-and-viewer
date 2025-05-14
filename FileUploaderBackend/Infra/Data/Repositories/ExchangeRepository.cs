using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ExchangeRepository : IExchangeRepository
{
    private readonly DbSet<Exchange> _dbSet;

    public ExchangeRepository(FileUploaderContext context)
    {
        _dbSet = context.Exchanges;
    }

    public async Task CreateBatch(IList<Exchange> exchanges)
    {
        await _dbSet.AddRangeAsync(exchanges);
    }

    public async Task DeleteAllFromJob(int jobId)
    {
        await _dbSet.Where(x => x.JobId == jobId).ExecuteDeleteAsync();
    }
}
