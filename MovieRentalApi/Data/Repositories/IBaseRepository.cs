namespace MovieRentalApi.Data.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
        Task<bool> UpdateAsync(TEntity entity);
    }
}
