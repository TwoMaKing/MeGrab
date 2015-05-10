using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Caches
{
    public class RedisCacheKeyGenerator : ICacheKeyGenerator
    {
        /// <summary>
        /// 获取缓存键
        /// </summary>
        /// <param name="key">缓存键</param>
        public string GetKey(string key)
        {
            return key;
        }

        /// <summary>
        /// 获取缓存过期标记键
        /// </summary>
        /// <param name="key">缓存键</param>
        public string GetSignKey(string key)
        {
            return string.Format("{0}_Sign", key);
        }
    }
}
