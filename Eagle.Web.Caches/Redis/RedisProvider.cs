using Eagle.Common.Serialization;
using Eagle.Core.Application;
using Eagle.Core.Exceptions;
using Eagle.Core.Extensions;
using ServiceStack.Caching;
using ServiceStack.Data;
using ServiceStack.Model;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using ServiceStack.Redis.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eagle.Web.Caches
{
    public class RedisProvider : ICacheProvider
    {
        private int expire = 3600;
        private int maxWritePoolSize = 60;
        private int maxReadPoolSize = 60;
        private bool autoStart = true;

        private string[] writeHosts = new string[] { "127.0.0.1:6379" };
        private string[] readOnlyHosts = new string[] { "127.0.0.1:6379" };

        private static MethodInfo replaceMethod;
        private static readonly object lockObject = new object();

        static RedisProvider()
        {
            if (replaceMethod == null)
            {
                lock (lockObject)
                {
                    if (replaceMethod == null)
                    {
                        MethodInfo[] allMethods = typeof(RedisClient).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                        var replaceMethodInfos = from m in allMethods
                                                 let name = m.Name
                                                 let parameters = m.GetParameters()
                                                 where name == "Replace" &&
                                                       parameters != null &&
                                                       parameters.Count() == 3 &&
                                                       parameters[2].ParameterType.Equals(typeof(DateTime))
                                                 select new { MethodInfo = m };

                        replaceMethod = replaceMethodInfos.SingleOrDefault().MethodInfo;
                    }
                }
            }
        }

        public RedisProvider()
        {
            this.expire = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.TimeOutSeconds;
            this.maxWritePoolSize = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.MaxWritePoolSize;
            this.maxReadPoolSize = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.MaxReadPoolSize;
            this.autoStart = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.AutoStart;

            string writeServerList = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.WriteHosts;
            string readOnlyServerList = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.ReadOnlyHosts;

            if (writeServerList.HasValue())
            {
                this.writeHosts = writeServerList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (readOnlyServerList.HasValue())
            {
                this.readOnlyHosts = readOnlyServerList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        private RedisClient CreateRedisClient() 
        {
            RedisClientManagerConfig config = new RedisClientManagerConfig();
            config.MaxWritePoolSize = maxWritePoolSize;
            config.MaxReadPoolSize = maxReadPoolSize;
            config.AutoStart = autoStart;

            PooledRedisClientManager redisClientManager = new PooledRedisClientManager(writeHosts, readOnlyHosts, config);
            return (RedisClient)redisClientManager.GetClient();
        }

        public void Add(string key, object item)
        {
            this.Add(key, item, this.expire);
        }

        public void Add(string key, object item, int expire)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                byte[] objectBytes = SerializationManager.SerializeToBinary(item);

                redisClient.Set(key, objectBytes, DateTime.Now.AddSeconds(expire));
            }
        }

        public void Add<T>(string key, T item)
        {
            this.Add<T>(key, item, this.expire);
        }

        public void Add<T>(string key, T[] items)
        {
            this.Add<T>(key, items, this.expire);
        }

        public void Add<T>(string key, T item, int expire)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                redisClient.Set<T>(key, item, DateTime.Now.AddSeconds(expire));
            }
        }

        public void Add<T>(string key, IEnumerable<T> items, int expire)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                IRedisTypedClient<T> redisTypeClient = redisClient.As<T>();

                for (int itemIndex = 0; itemIndex < items.Count(); itemIndex++)
                {
                    redisTypeClient.AddItemToSortedSet(redisTypeClient.SortedSets[key], items.ElementAt(itemIndex));
                }
                redisTypeClient.ExpireEntryAt(key, DateTime.Now.AddSeconds(expire));
            }
        }

        public void Replace(string key, object item)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                Type itemType = item.GetType();
                MethodInfo genericReplaceMethod = replaceMethod.MakeGenericMethod(itemType);
                genericReplaceMethod.Invoke(redisClient, new object[] { key, item });
            }
        }

        public void Replace<T>(string key, T item)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                redisClient.Replace<T>(key, item);
            }
        }

        public void Replace(string key, object item, int expire)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                Type itemType = item.GetType();
                MethodInfo genericReplaceMethod = replaceMethod.MakeGenericMethod(itemType);
                genericReplaceMethod.Invoke(redisClient, new object[] { key, item, DateTime.Now.AddSeconds(expire) });
            }
        }

        public void Replace<T>(string key, T item, int expire)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                redisClient.Replace<T>(key, item, DateTime.Now.AddSeconds(expire));
            }
        }

        public void Replace<T>(string key, IEnumerable<T> item, int expire)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                return redisClient.ContainsKey(key);
            }
        }

        public object Get(string key)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                byte[] objectBytes = redisClient.Get(key);

                return SerializationManager.DeserializeFromBinary(objectBytes);
            }
        }

        public T GetItem<T>(string key)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                return redisClient.Get<T>(key);
            }
        }

        public IEnumerable<T> GetItems<T>(string key)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                IRedisTypedClient<T> redisTypeClient = redisClient.As<T>();

                return redisTypeClient.GetAllItemsFromSortedSet(redisTypeClient.SortedSets[key]);
            }
        }

        public void Remove(string key)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                redisClient.Remove(key);
            }
        }

        public void FlushAll()
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                redisClient.FlushAll();
            }
        }
        
        public T GetCacheProvider<T>() where T : class
        {
            if (typeof(T).IsAssignableFrom(typeof(RedisClient)))
            {
                using (RedisClient redisClient = this.CreateRedisClient())
                {
                    return redisClient as T;
                }
            }

            throw new InfrastructureException("The cache provider type provided by the current cache manager should be '{0}'.", typeof(IRedisClient));
        }

        public void Dispose() { }

    }
}
