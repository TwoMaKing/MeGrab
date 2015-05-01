using Eagle.Core.Exceptions;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Caches
{
    public class MemcachedProvider : ICacheProvider
    {
        private static MemcachedClient memcachedClient = new MemcachedClient("enyim.com/memcached");  

        public void Add(string key, object item)
        {
            memcachedClient.Store(StoreMode.Add, key, item);
        }

        public void Add(string key, object item, int expire)
        {
            memcachedClient.Store(StoreMode.Add, key, item, DateTime.Now.AddSeconds(expire));
        }

        public void Add<T>(string key, T item)
        {
            memcachedClient.Store(StoreMode.Add, key, item);
        }

        public void Add<T>(string key, T[] items)
        {
            memcachedClient.Store(StoreMode.Add, key, items);
        }

        public void Add<T>(string key, T item, int expire)
        {
            memcachedClient.Store(StoreMode.Add, key, item, DateTime.Now.AddSeconds(expire));
        }

        public void Add<T>(string key, IEnumerable<T> items, int expire)
        {
            memcachedClient.Store(StoreMode.Add, key, items, DateTime.Now.AddSeconds(expire));
        }

        public void Replace(string key, object item)
        {
            memcachedClient.Store(StoreMode.Replace, key, item);
        }

        public void Replace<T>(string key, T item)
        {
            memcachedClient.Store(StoreMode.Replace, key, item);
        }

        public void Replace(string key, object item, int expire)
        {
            memcachedClient.Store(StoreMode.Replace, key, item, DateTime.Now.AddSeconds(expire));
        }

        public void Replace<T>(string key, T item, int expire)
        {
            memcachedClient.Store(StoreMode.Replace, key, item, DateTime.Now.AddSeconds(expire));
        }

        public bool ContainsKey(string key)
        {
            return memcachedClient.CheckAndSet(key, new object(), 0);
        }

        public object Get(string key)
        {
            return memcachedClient.Get(key);
        }

        public T GetItem<T>(string key)
        {
            return memcachedClient.Get<T>(key);
        }

        public IEnumerable<T> GetItems<T>(string key)
        {
            var items = memcachedClient.Get(key);

            if (items == null)
            {
                return null;
            }

            if (typeof(IEnumerable<T>).IsAssignableFrom(items.GetType()))
            {
                return (IEnumerable<T>)items;
            }

            return null;
        }

        public void Remove(string key)
        {
            memcachedClient.Remove(key);
        }

        public void FlushAll()
        {
            memcachedClient.FlushAll();
        }

        public T GetCacheProvider<T>() where T : class
        {
            if (typeof(T).IsAssignableFrom(typeof(MemcachedClient)))
            {
                return memcachedClient as T;
            }

            throw new InfrastructureException("The cache provider type provided by the current cache manager should be '{0}'.", typeof(MemcachedClient));
        }

        public void Dispose()
        {
            memcachedClient.Dispose();
        }
    }
}
