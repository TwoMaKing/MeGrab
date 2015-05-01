using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Caches
{
    /// <summary>
    /// 缓存管理器
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// 得到缓存提供器
        /// </summary>
        ICacheProvider CacheProvider { get; }

        /// <summary>
        /// 得到配置缓存的键的产生器
        /// </summary>
        ICacheKeyGenerator CacheKeyGenerator { get; }

        /// <summary>
        /// 获取缓存对象，如果缓存对象不存在，则执行retrieveFunc获取对象并添加到缓存中
        /// </summary>
        ///<param name="key">缓存键</param>
        ///<param name="retrieveFunc">获取要缓存的对象的方法， 如果缓存对象不存在，则执行该retrieveFunc获得对象并添加到缓存中</param>
        ///<param name="expire">过期时间，秒</param>
        T Get<T>(string key, Func<T> retrieveFunc, int expire = 0);

        /// <summary>
        /// 获取缓存对象集合，如果缓存对象集合不存在，则执行retrieveFunc获取对象集合并添加到缓存中
        /// </summary>
        ///<param name="key">缓存键</param>
        ///<param name="retrieveFunc">获取要缓存的对象集合的方法， 如果缓存对象不存在，则执行该retrieveFunc获得对象集合并添加到缓存中</param>
        ///<param name="expire">过期时间，秒</param>
        IEnumerable<T> Get<T>(string key, Func<IEnumerable<T>> retrieveFunc, int expire = 0);

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        /// <param name="expire">过期时间，秒</param>
        void Update(string key, object target, int expire = 0);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        void Remove(string key);
    }
}
