namespace Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveAsync();

    void BeginTransaction();

    void CommitTransaction();

    void RollbackTransaction();
}
