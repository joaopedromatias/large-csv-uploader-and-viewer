using Data.Context;
using Domain.Interfaces.UnitOfWork;

namespace Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly FileUploaderContext _context;

    public UnitOfWork(FileUploaderContext context)
    {
        _context = context;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public void BeginTransaction()
    {
        _context.Database.BeginTransactionAsync();
    }

    public void CommitTransaction()
    {
        _context.Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        _context.Database.RollbackTransaction();
    }
}
