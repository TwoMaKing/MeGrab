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
    public class RedisManager : ICacheManager
    {
        private int expire = 3600;
        private int maxWritePoolSize = 60;
        private int maxReadPoolSize = 60;
        private bool autoStart = true;

        private string[] writeHosts = new string[] { "127.0.0.1:6379" };
        private string[] readOnlyHosts = new string[] { "127.0.0.1:6379" };

        private static MethodInfo replaceMethod;

        static RedisManager()
        {
            //var replaceMethodInfos = from m in typeof(RedisClient).GetMethods()
            //                         let name = m.Name
            //                         let parameters = m.GetParameters()
            //                         where name == "Replace" && parameters.Count() > 3 &&
            //                               parameters[2].ParameterType == typeof(DateTime)
            //                         select m;
                                      
            //replaceMethod = replaceMethodInfos.SingleOrDefault();
        }

        public RedisManager()
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

        public void AddItem(string key, object item)
        {
            this.AddItem(key, item, this.expire);
        }

        public void AddItem(string key, object item, int expire)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                byte[] objectBytes = SerializationManager.SerializeToBinary(item);

                redisClient.Set(key, objectBytes, DateTime.Now.AddSeconds(expire));
            }
        }

        public void AddItem<T>(string key, T item)
        {
            this.AddItem<T>(key, item, this.expire);
        }

        public void AddItem<T>(string key, T item, int expire)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                redisClient.Set<T>(key, item, DateTime.Now.AddSeconds(expire));
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

        public bool ContainsKey(string key)
        {
            using (RedisClient redisClient = this.CreateRedisClient())
            {
                return redisClient.ContainsKey(key);
            }
        }

        public object GetItem(string key)
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

        public void RemoveItem(string key)
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
