using Domain.Models;

namespace Domain.Interfaces.Repositories;

public interface IJobRepository
{
    Task Create(Job job);
}
