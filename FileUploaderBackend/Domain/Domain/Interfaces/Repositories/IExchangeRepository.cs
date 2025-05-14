using Domain.Models;

namespace Domain.Interfaces.Repositories;

public interface IExchangeRepository
{
    Task CreateBatch(IList<Exchange> exchanges);
    Task DeleteAllFromJob(int jobId);
}