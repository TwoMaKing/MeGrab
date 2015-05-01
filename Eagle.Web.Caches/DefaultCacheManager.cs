using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Caches
{
    public class DefaultCacheManager : CacheManagerBase
    {
        public DefaultCacheManager() : base(CacheProviderFactory.GetCacheProvider(), new RedisCacheKeyGenerator()) { }
    }
}
