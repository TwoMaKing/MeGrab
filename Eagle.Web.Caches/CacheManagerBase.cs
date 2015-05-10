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
        protected const string cacheSignValue = "SIGN";

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

        /// <summary>
        /// 获取缓存对象，如果缓存对象不存在，则执行retrieveFunc获取对象并添加到缓存中
        /// </summary>
        ///<param name="key">缓存键</param>
        ///<param name="retrieveFunc">获取要缓存的对象的方法， 如果缓存对象不存在，则执行该retrieveFunc获得对象并添加到缓存中</param>
        ///<param name="expire">过期时间，秒</param>
        public T Get<T>(string key, Func<T> retrieveFunc, int expire = 0)
        {
            string cacheKey = this.GetCacheKey(key);

            // 缓存标记
            string cacheSignKey = this.GetSignKey(key);

            var sign = this.CacheProvider.GetItem<string>(cacheSignKey);

            var cacheItem = this.CacheProvider.GetItem<T>(cacheKey);

            if (sign != null)
            {
                return cacheItem;
            }

            lock (cacheSignKey)
            {
                sign = this.CacheProvider.GetItem<string>(cacheSignKey);
                if (sign != null)
                {
                    return cacheItem;
                }

                this.CacheProvider.Add<string>(cacheSignKey, cacheSignValue, this.GetSignCacheExpireTime(expire));

                return this.AddOrUpdateCache<T>(retrieveFunc, cacheKey, expire, cacheItem);
            }
        }

        /// <summary>
        /// 获取缓存对象集合，如果缓存对象集合不存在，则执行retrieveFunc获取对象集合并添加到缓存中
        /// </summary>
        ///<param name="key">缓存键</param>
        ///<param name="retrieveFunc">获取要缓存的对象集合的方法， 如果缓存对象不存在，则执行该retrieveFunc获得对象集合并添加到缓存中</param>
        ///<param name="expire">过期时间，秒</param>
        public IEnumerable<T> Get<T>(string key, Func<IEnumerable<T>> retrieveFunc, int expire = 0)
        {
            string cacheKey = this.GetCacheKey(key);

            // 缓存标记
            string cacheSignKey = this.GetSignKey(key);

            var sign = this.CacheProvider.Get(cacheSignKey);

            var cacheItems = this.CacheProvider.GetItems<T>(cacheKey);

            if (sign != null)
            {
                return cacheItems;
            }

            lock (cacheSignKey)
            {
                sign = this.CacheProvider.Get(cacheSignKey);
                if (sign != null)
                {
                    return cacheItems;
                }

                this.CacheProvider.Add(cacheSignKey, cacheSignValue, this.GetSignCacheExpireTime(expire));
                return this.AddOrUpdateCache<T>(retrieveFunc, cacheKey, expire, cacheItems);
            }
        }

        public void Update(string key, object target, int expire = 0)
        {
            string cacheKey = this.GetCacheKey(key);

            if (!this.CacheProvider.ContainsKey(cacheKey))
            {
                return;
            }

            string signCacheKey = this.GetSignKey(key);
            string lockKey = this.GetLockKey(key);

            lock (lockKey)
            {

                if (this.CacheProvider.ContainsKey(cacheKey))
                {
                    this.CacheProvider.Replace(signCacheKey, cacheSignValue, this.GetSignCacheExpireTime(expire));
                    this.CacheProvider.Replace(cacheKey, target, expire);
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

            string signCacheKey = this.GetSignKey(key);
            string lockKey = this.GetLockKey(key);

            lock (lockKey)
            {
                this.CacheProvider.Remove(signCacheKey);
                this.CacheProvider.Remove(cacheKey);
            }
        }

        protected string GetCacheKey(string key)
        {
            return this.CacheKeyGenerator.GetKey(key);
        }

        /// <summary>
        /// 获取缓存过期标记
        /// </summary>
        protected string GetSignKey(string key)
        {
            return string.Intern(this.CacheKeyGenerator.GetSignKey(key));
        }

        protected string GetLockKey(string key)
        {
            return string.Intern(string.Format("{0}_n(*≧▽≦*)n", key));
        }

        protected int GetSignCacheExpireTime(int expire)
        {
            return Convertor.ConvertToInteger(expire / 2).Value;
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        protected T AddOrUpdateCache<T>(Func<T> retrieveFunc, string cacheKey, int expire, T cacheItem)
        {
            if (Equals(cacheItem, null))
            {
                cacheItem = retrieveFunc();
                this.CacheProvider.Add<T>(cacheKey, cacheItem, expire);
                return cacheItem;
            }

            Task t = Task.Factory.StartNew(() => this.CacheProvider.Replace<T>(cacheKey, retrieveFunc(), expire));

            return cacheItem;
        }

        /// <summary>
        /// 更新缓存集合
        /// </summary>
        protected IEnumerable<T> AddOrUpdateCache<T>(Func<IEnumerable<T>> retrieveFunc, string cacheKey, int expire, IEnumerable<T> cacheItems)
        {
            if (Equals(cacheItems, null))
            {
                cacheItems = retrieveFunc();
                this.CacheProvider.Add<T>(cacheKey, cacheItems, expire);
                return cacheItems;
            }

            Task t = Task.Factory.StartNew(() => this.CacheProvider.Replace<T>(cacheKey, retrieveFunc(), expire));

            return cacheItems;
        }
    }
}
