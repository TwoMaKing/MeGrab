using Eagle.Common.Util;
using Eagle.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eagle.Web.Caches
{
    public abstract class CacheManagerBase : ICacheManager
    {
        protected CacheManagerBase(ICacheProvider cacheProvider, ICacheKeyGenerator cacheKey) 
        {
            Guard.NotNull(cacheProvider, "Cache Provider");
            Guard.NotNull(cacheKey, "Cache Key");

            this.CacheProvider = cacheProvider;
            this.CacheKeyGenerator = cacheKey;
        }

        public ICacheProvider CacheProvider
        {
            get;
            protected set;
        }

        public ICacheKeyGenerator CacheKeyGenerator 
        { 
            get; 
            protected set; 
        }

        public T Get<T>(string key, Func<T> retrieveFunc, int expire = 0)
        {
            string cacheKey = this.GetCacheKey(key);

            // 缓存标记
            string cacheSign = string.Format("{0}_Sign", cacheKey);
            var sign = this.CacheProvider.GetItem<string>(cacheSign);

            var cacheItem = this.CacheProvider.GetItem<T>(cacheKey);

            if (sign != null)
            {
                return cacheItem;
            }

            lock (cacheSign)
            {
                sign = this.CacheProvider.GetItem<string>(cacheSign);
                if (sign != null)
                {
                    return cacheItem;
                }

                this.CacheProvider.Add<string>(cacheSign, "Sign", (Convertor.ConvertToInteger(expire / 2).Value));

                Task t = Task.Factory.StartNew(() =>
                {
                    cacheItem = retrieveFunc();
                    this.CacheProvider.Add<T>(cacheKey, cacheItem, expire);
                });

                t.Wait();

            }

            return cacheItem;
        }

        public IEnumerable<T> Get<T>(string key, Func<IEnumerable<T>> retrieveFunc, int expire = 0)
        {
            string cacheKey = this.GetCacheKey(key);
            
            // 缓存标记
            string cacheSign = string.Format("{0}_Sign", cacheKey);
            var sign = this.CacheProvider.Get(cacheSign);

            var cacheItems = this.CacheProvider.GetItems<T>(cacheKey);

            if (sign != null)
            {
                return cacheItems;
            }

            lock (cacheSign)
            {
                sign = this.CacheProvider.Get(cacheSign);
                if (sign != null)
                {
                    return cacheItems;
                }

                this.CacheProvider.Add(cacheSign, "Sign", (Convertor.ConvertToInteger(expire / 2).Value));

                Task t = Task.Factory.StartNew(() =>
                {
                    cacheItems = retrieveFunc();
                    this.CacheProvider.Add<T>(cacheKey, cacheItems, expire);
                });

                t.Wait();
            }

            return cacheItems;
        }

        public void Update(string key, object target, int expire = 0)
        {
            string cacheKey = this.GetCacheKey(key);

            if (!this.CacheProvider.ContainsKey(cacheKey))
            {
                return;
            }

            string lockKey = cacheKey + "n(*≧▽≦*)n";

            lock (lockKey)
            {
                if (this.CacheProvider.ContainsKey(cacheKey))
                {
                    this.CacheProvider.Replace(cacheKey, target);
                }
            }
        }

        public void Remove(string key)
        {
            string cacheKey = this.GetCacheKey(key);

            if (!this.CacheProvider.ContainsKey(cacheKey))
            {
                return;
            }

            string lockKey = cacheKey + "n(*≧▽≦*)n";

            lock (lockKey)
            {
                if (this.CacheProvider.ContainsKey(cacheKey))
                {
                    this.CacheProvider.Remove(cacheKey);
                }
            }
        }

        protected string GetCacheKey(string key)
        {
            return this.CacheKeyGenerator.GetKey(key);
        }

    }
}
