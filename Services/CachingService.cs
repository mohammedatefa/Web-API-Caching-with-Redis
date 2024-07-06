using StackExchange.Redis;
using System.Text.Json;

namespace Caching_with_Redis.Services
{
    public class CachingService : ICachingService
    {
        private IDatabase _cachDb;
        public CachingService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cachDb = redis.GetDatabase();
        }

        public async Task<T> GetData<T>(string key)
        {
            var values = await _cachDb.StringGetAsync(key);
            if (!string.IsNullOrEmpty(values))
                return JsonSerializer.Deserialize<T>(values);
            return default;
        }

        public async Task<object> RemoveData(string key)
        {
            var exist = await _cachDb.KeyExistsAsync(key);
            if (exist)
                return await _cachDb.KeyDeleteAsync(key);
            return false;
        }

        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationDateTime)
        {
            var expireTime = expirationDateTime.DateTime.Subtract(DateTime.Now);
            var isSet = await _cachDb.StringSetAsync(key, JsonSerializer.Serialize(value), expireTime);
            return isSet;
            
        }

        public async Task<bool> UpdateData<T>(string key, T value, DateTimeOffset expirationDateTime)
        {
            var exist = await _cachDb.KeyExistsAsync(key);
            if (exist)
            {
                return await SetData(key, value, expirationDateTime);
            }
            return false;
        }
    }
}
