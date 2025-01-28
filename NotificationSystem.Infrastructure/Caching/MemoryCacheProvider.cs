using Microsoft.Extensions.Caching.Memory;
using NotificationSystem.Contracts.Infrastructure;

namespace NotificationSystem.Infrastructure.Caching
{
    //Faking a Redis Cache Provider
    public class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;
        private static readonly TimeSpan DefaultTtl = TimeSpan.FromHours(24);

        public MemoryCacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void StoreAsync<T>(object key, T value) where T : class
        {
            StoreAsync(key, value, DefaultTtl);
        }

        public void StoreAsync<T>(object key, T value, TimeSpan ttl) where T : class
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _memoryCache.Set(key, value, DateTimeOffset.UtcNow.Add(ttl));
        }

        public T RetrieveAsync<T>(object key) where T : class
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var cachedObject = _memoryCache.Get(key) as T;

            return cachedObject;
        }
    }
}
