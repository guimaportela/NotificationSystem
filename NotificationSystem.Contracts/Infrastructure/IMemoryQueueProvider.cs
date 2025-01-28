using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSystem.Contracts.Infrastructure
{
    public interface IMemoryQueueProvider
    {
        void StoreAsync<T>(object key, T value) where T : class;
        void StoreAsync<T>(object key, T value, TimeSpan ttl) where T : class;
        T RetrieveAsync<T>(object key) where T : class;
    }
}
