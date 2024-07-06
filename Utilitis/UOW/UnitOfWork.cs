using Caching_with_Redis.Context;
using Caching_with_Redis.Repositories;
using Caching_with_Redis.Services;
using System.Collections;

namespace Caching_with_Redis.Utalitis.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SalseManagementDbContext _SalseManagementContext;
        private readonly ICachingService cachingService;
        private Hashtable Repositories;
        
        public UnitOfWork(SalseManagementDbContext SalseManagementContext,ICachingService cachingService)
        {
            _SalseManagementContext = SalseManagementContext;
            this.cachingService = cachingService;
            Repositories = new Hashtable();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;
            if (!Repositories.ContainsKey(type))
            {
                var repo = new GenericRepository<T>(_SalseManagementContext, cachingService);
                Repositories.Add(type, repo);
            }
            return Repositories[type] as IGenericRepository<T>;
        }

        public async Task<int> SaveChanges()
             => await _SalseManagementContext.SaveChangesAsync();
        public async ValueTask DisposeAsync()
             => await _SalseManagementContext.DisposeAsync();
    }
}
