namespace BureauHexagonal.Application.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();
        Task RollbackAsync();
        Task<int> CommitAsync();
        Task<int> SaveIntermediateAsync();
    }
}
