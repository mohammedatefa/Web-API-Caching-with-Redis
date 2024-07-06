namespace Caching_with_Redis.Services
{
    public interface ICachingService
    {
        public Task<T> GetData<T>(string key);
        public Task<bool> SetData<T>(string key, T value,DateTimeOffset expirationDateTime);
        public Task<bool> UpdateData<T>(string key, T value, DateTimeOffset expirationDateTime);
        public Task<object> RemoveData(string key);
    }
}
