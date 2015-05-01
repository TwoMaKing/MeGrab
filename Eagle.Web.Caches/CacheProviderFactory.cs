using Eagle.Core.Application;
using Eagle.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Web.Caches
{
    public sealed class CacheProviderFactory
    {
        private static Dictionary<string, ICacheProvider> cacheProviderDictionary = new Dictionary<string, ICacheProvider>();

        private static readonly object lockObject = new object();

        private CacheProviderFactory() { }

        public static ICacheProvider GetCacheProvider() 
        {
            string defaultCacheProvider = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.CacheManagers.Default;

            if (string.IsNullOrEmpty(defaultCacheProvider) ||
                string.IsNullOrWhiteSpace(defaultCacheProvider))
            {
                defaultCacheProvider = "Redis";
            }

            return GetCacheProvider(defaultCacheProvider);
        }

        public static ICacheProvider GetCacheProvider(string name)
        {
            string cacheProviderTypeName = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.CacheManagers[name].Type;

            if (cacheProviderDictionary.ContainsKey(cacheProviderTypeName))
            {
                return (ICacheProvider)cacheProviderDictionary[cacheProviderTypeName];
            }

            if (string.IsNullOrEmpty(cacheProviderTypeName))
            {
                throw new ConfigException("The cache manager has not been defined in the ConfigSource.");
            }

            Type cacheProviderType = Type.GetType(cacheProviderTypeName);

            if (cacheProviderType == null)
            {
                throw new InfrastructureException("The Cache Manager defined by the type {0} doesn't exist.", cacheProviderTypeName);
            }

            if (!typeof(ICacheProvider).IsAssignableFrom(cacheProviderType))
            {
                throw new ConfigException("Type '{0}' is not a Cache Manager.", cacheProviderType);
            }

            ICacheProvider cacheProvider;

            lock (lockObject)
            {
                if (!cacheProviderDictionary.ContainsKey(cacheProviderTypeName))
                {
                    cacheProvider = (ICacheProvider)AppRuntime.Instance.CurrentApplication.ObjectContainer.Resolve(cacheProviderType,
                                                                                                                   cacheProviderTypeName);

                    cacheProviderDictionary.Add(cacheProviderTypeName, cacheProvider);
                }
                else
                {
                    cacheProvider = (ICacheProvider)cacheProviderDictionary[cacheProviderTypeName];
                }
            }

            return cacheProvider;
        }

    }
}
