using Microsoft.EntityFrameworkCore.Storage;

namespace MovieRentalApi.Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
	Task CreateAsync(TEntity entity);
	Task<TEntity?> GetByIdAsync(int id);
	Task<bool> UpdateAsync(TEntity entity);
	Task<IDbContextTransaction> BeginTransactionAsync();
	Task CommitAsync(IDbContextTransaction transaction);
}