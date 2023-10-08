using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieRentalApi.Data.DbContexts;

namespace MovieRentalApi.Data.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
	private readonly MovieDbContext dbContext;
	private readonly DbSet<TEntity> dbSet;
	private bool isTransactional;

	public BaseRepository(MovieDbContext dbContext)
	{
		this.dbContext = dbContext;
		dbSet = dbContext.Set<TEntity>();
		isTransactional = false;
	}

	public async Task<TEntity?> GetByIdAsync(int id)
	{
		var entity = await dbSet.FindAsync(id);
		return entity;
	}

	public async Task CreateAsync(TEntity entity)
	{
		await dbSet.AddAsync(entity);
		if (!isTransactional)
			await dbContext.SaveChangesAsync();
	}

	public async Task<bool> UpdateAsync(TEntity entity)
	{
		dbSet.Update(entity);
		if (!isTransactional)
			return await dbContext.SaveChangesAsync() > 0;

		return false;
	}

	public async Task<IDbContextTransaction> BeginTransactionAsync()
	{
		isTransactional = true;
		return await dbContext.Database.BeginTransactionAsync();
	}

	public async Task CommitAsync(IDbContextTransaction transaction)
	{
		try
		{
			if (isTransactional) await transaction.CommitAsync();
		}
		catch
		{
			await transaction.RollbackAsync();
			throw;
		}
		finally
		{
			await transaction.DisposeAsync();
		}
	}
}