using Caching_with_Redis.Context;
using Caching_with_Redis.Services;
using Microsoft.EntityFrameworkCore;
namespace Caching_with_Redis.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal SalseManagementDbContext _context;
        internal ICachingService _cachingDb;
        internal DbSet<T> _dbset;
        public GenericRepository(SalseManagementDbContext context,ICachingService cachingDb)
        {
            _context = context;
            _cachingDb = cachingDb;
            _dbset = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(string includeProperties = "")
        {
            //using Redis Caching 
            var cachData = await _cachingDb.GetData<IEnumerable<T>>(typeof(T).Name);

            if (cachData != null && cachData.Any())
            {
                return cachData;
            }
            var query = _dbset.AsQueryable();
           
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var data = await query.ToListAsync();

            //Add to Caching 
            var expireTime = DateTimeOffset.Now.AddSeconds(120);
            await _cachingDb.SetData<IEnumerable<T>>(typeof(T).Name, data, expireTime);

            return data;
        }

        public async Task<T> GetByIdAsync(object Id, string includeProperties = "")
        {
            var cachedData = await _cachingDb.GetData<IEnumerable<T>>(typeof(T).Name);
            if (cachedData != null && cachedData.Any())
            {
                var cachedItem = cachedData.FirstOrDefault(e => typeof(T).GetProperty("Id").GetValue(e).Equals(Id));

                if (cachedItem != null)
                {
                    return cachedItem;
                }
            }

            IQueryable<T> query = _dbset.AsQueryable();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var result = await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(Id));
            if (result != null)
            {
                var expireTime = DateTimeOffset.Now.AddSeconds(120);
                await _cachingDb.SetData<T>($"{typeof(T).Name}{Id}", result, expireTime);
            }

            return result;
        }


        public async Task AddAsync(T entity)
        {
            var resualt =await _dbset.AddAsync(entity);
            //add to caching data
            if(resualt != null) {
                var expirtime = DateTimeOffset.Now.AddSeconds(10);
                await _cachingDb.SetData($"{typeof(T).Name}{typeof(T).GetProperty("Id").GetValue(resualt.Entity)}", entity, expirtime);
            }
            
        }

        public async Task DeleteAsync(object Id)
        {
            var entity = await _dbset.FindAsync(Id);
            if (entity != null) {
                _dbset.Remove(entity);

                //remove from caching data 
                var entitttype = entity.GetType().Name;
                await _cachingDb.RemoveData($"{entitttype}{Id}");
            }        
        }

        public async Task UpdateAsync(object Id, T entity)
        {
            var entityToUpdate = await _dbset.FindAsync(Id);
            if (entityToUpdate != null)
                _dbset.Update(entity);

            var expireTime = DateTimeOffset.Now.AddSeconds(120);
            await _cachingDb.UpdateData($"{typeof(T).Name}{Id}", entity, expireTime);
        }
    }
}
