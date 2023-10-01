using Microsoft.EntityFrameworkCore;
using MovieRentalApi.Data.DbContexts;

namespace MovieRentalApi.Data.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
	protected readonly MovieDbContext DbContext;
	protected readonly DbSet<TEntity> DbSet;

	public BaseRepository(MovieDbContext dbContext)
	{
		DbContext = dbContext;
		DbSet = dbContext.Set<TEntity>();
	}

	public async Task<TEntity> CreateAsync(TEntity entity)
	{
		await DbContext.Set<TEntity>().AddAsync(entity);
		await DbContext.SaveChangesAsync();
		return entity;
	}

	public async Task<TEntity> GetByIdAsync(int id)
	{
		var entity = await DbContext.Set<TEntity>().FindAsync(id);
		return entity;
	}

	public async Task<bool> UpdateAsync(TEntity entity)
	{
		DbContext.Entry(entity).State = EntityState.Modified;
		return await DbContext.SaveChangesAsync() > 0;
	}
}