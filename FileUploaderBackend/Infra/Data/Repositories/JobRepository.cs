using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class JobRepository : IJobRepository
{
    private readonly DbSet<Job> _dbSet;

    public JobRepository(FileUploaderContext context)
    {
        _dbSet = context.Jobs;
    }

    public async Task Create(Job job)
    {
        await _dbSet.AddAsync(job);
    }
}
