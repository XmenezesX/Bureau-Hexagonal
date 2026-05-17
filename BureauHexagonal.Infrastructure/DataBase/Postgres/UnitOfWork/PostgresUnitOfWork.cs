using BureauHexagonal.Application.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace BureauHexagonal.Infrastructure.DataBase.Postgres.UnitOfWork
{
    public sealed class PostgresUnitOfWork : IUnitOfWork
    {
        private readonly BureauPostgresDbContext _dbContext;
        private IDbContextTransaction _transaction;
        private int _Rows {  get; set; }
        public PostgresUnitOfWork(BureauPostgresDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BeginTransactionAsync()
        {
            try
            {
                if (_transaction is not null)
                    return;

                _transaction = await _dbContext.Database.BeginTransactionAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                int rows = await _dbContext.SaveChangesAsync();
                if (_transaction is not null)
                    await _transaction.CommitAsync();
                
                return rows;
            }
            catch (Exception ex)
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_transaction is not null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_transaction is not null)
                {
                    await _transaction.RollbackAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> SaveIntermediateAsync()
        {
            try
            {
                int rows = await _dbContext.SaveChangesAsync();
                return rows;
            }
            catch (Exception ex)
            {
                await RollbackAsync();
                throw;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
        }
    }
}
