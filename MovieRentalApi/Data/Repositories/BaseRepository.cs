using MovieRentalApi.Data.DbContexts;

namespace MovieRentalApi.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;

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
    }
}
