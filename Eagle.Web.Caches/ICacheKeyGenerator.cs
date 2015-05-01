using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Caches
{
    /// <summary>
    /// 代表缓存键的接口
    /// </summary>
    public interface ICacheKeyGenerator
    {
        /// <summary>
        /// 获取缓存键
        /// </summary>
        /// <param name="cache">缓存键</param>
        string GetKey(string key);
    }
}
