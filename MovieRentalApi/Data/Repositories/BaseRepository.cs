using Microsoft.EntityFrameworkCore;
using MovieRentalApi.Data.DbContexts;

namespace MovieRentalApi.Data.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
	private readonly MovieDbContext dbContext;
	protected readonly DbSet<TEntity> DbSet;

	public BaseRepository(MovieDbContext dbContext)
	{
		this.dbContext = dbContext;
		DbSet = dbContext.Set<TEntity>();
	}

	public async Task<TEntity> CreateAsync(TEntity entity)
	{
		await dbContext.Set<TEntity>().AddAsync(entity);
		await dbContext.SaveChangesAsync();
		return entity;
	}

	public async Task<TEntity?> GetByIdAsync(int id)
	{
		var entity = await dbContext.Set<TEntity>().FindAsync(id);
		return entity;
	}

	public async Task<bool> UpdateAsync(TEntity entity)
	{
		dbContext.Entry(entity).State = EntityState.Modified;
		return await dbContext.SaveChangesAsync() > 0;
	}
}