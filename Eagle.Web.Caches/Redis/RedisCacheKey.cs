using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Caches
{
    public class RedisCacheKeyGenerator : ICacheKeyGenerator
    {
        public string GetKey(string key)
        {
            return key;
        }
    }
}
