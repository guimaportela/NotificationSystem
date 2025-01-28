using Microsoft.Extensions.Caching.Memory;
using NotificationSystem.Contracts.Infrastructure;

namespace NotificationSystem.Infrastructure.Queueing
{
    public class MemoryQueueProvider : IMemoryQueueProvider
    {
        private readonly IMemoryCache _memoryCache;
        private static readonly TimeSpan DefaultTtl = TimeSpan.FromHours(3);

        public MemoryQueueProvider(IMemoryCache memoryCache)
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

            var cachedObject = _memoryCache.Get(key) as Queue<T>;

            if (cachedObject == null)
            {
                cachedObject = new Queue<T>();
                _memoryCache.Set(key, cachedObject, DateTimeOffset.UtcNow.Add(ttl));
            }

            cachedObject.Enqueue(value);

            _memoryCache.Set(key, cachedObject, DateTimeOffset.UtcNow.Add(DefaultTtl));
        }

        public T RetrieveAsync<T>(object key) where T : class
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var cachedObject = _memoryCache.Get(key) as Queue<T>;

            if (cachedObject == null || !cachedObject.Any())
                return default(T);

            var obj = cachedObject.Dequeue();
            _memoryCache.Set(key, cachedObject, DateTimeOffset.UtcNow.Add(DefaultTtl));

            return obj;
        }
    }
}
