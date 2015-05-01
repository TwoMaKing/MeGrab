using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Web.Caches
{
    /// <summary>
    /// 缓存提供器，E.g. HttpCache, Memcached, Redis等.
    /// </summary>
    public interface ICacheProvider : IDisposable
    {
        /// <summary>
        /// 添加对象到缓存
        /// </summary>
        void Add(string key, object item);

        /// <summary>
        /// 添加对象到缓存指定到期时间
        /// </summary>
        void Add(string key, object item, int expire);

        /// <summary>
        /// 添加对象到缓存
        /// </summary>
        void Add<T>(string key, T item);

        /// <summary>
        /// 添加对象集合到缓存
        /// </summary>
        void Add<T>(string key, T[] items);

        /// <summary>
        /// 添加对象到缓存指定到期时间
        /// </summary>
        void Add<T>(string key, T item, int expire);

        /// <summary>
        /// 添加对象到缓存指定到期时间
        /// </summary>
        void Add<T>(string key, IEnumerable<T> items, int expire);

        /// <summary>
        /// 替换指定的缓存对象
        /// </summary>
        void Replace(string key, object item);

        /// <summary>
        /// 替换指定的缓存对象
        /// </summary>
        void Replace<T>(string key, T item);

        /// <summary>
        /// 替换指定的缓存对象
        /// </summary>
        void Replace(string key, object item, int expire);

        /// <summary>
        /// 替换指定的缓存对象
        /// </summary>
        void Replace<T>(string key, T item, int expire);

        /// <summary>
        /// 是否存在指定Key的Item
        /// </summary>
        bool ContainsKey(string key);

        /// <summary>
        /// 获取对象 By Key
        /// </summary>
        object Get(string key);

        /// <summary>
        /// 获取对象 By Key
        /// </summary>
        T GetItem<T>(string key);

        /// <summary>
        /// 获取对象集合 By Key
        /// </summary>
        IEnumerable<T> GetItems<T>(string key);

        /// <summary>
        /// 从缓存中移除
        /// </summary>
        void Remove(string key);

        /// <summary>
        /// 清空所有缓存对象
        /// </summary>
        void FlushAll();

        /// <summary>
        /// 得到 分布式 Cache 提供器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetCacheProvider<T>() where T : class;

    }
}
