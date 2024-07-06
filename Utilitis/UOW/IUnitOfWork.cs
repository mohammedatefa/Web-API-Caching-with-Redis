using Caching_with_Redis.Repositories;

namespace Caching_with_Redis.Utalitis.UOW
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChanges();
    }
}
