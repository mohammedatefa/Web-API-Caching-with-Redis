namespace Caching_with_Redis.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(string includeProperties="");
        public Task<TEntity> GetByIdAsync(object Id, string includeProperties="");
        public Task AddAsync(TEntity entity);
        public Task UpdateAsync(object Id, TEntity entity);
        public Task DeleteAsync(object Id);
    }
}
