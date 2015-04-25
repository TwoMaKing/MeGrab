using System;

namespace MeGrab.Domain.Models
{
    /// <summary>
    /// 派发模式
    /// </summary>
    [Serializable()]
    public enum DispatchMode
    {
        /// <summary>
        /// 固定金额或者数量
        /// </summary>
        Fixed = 0,

        /// <summary>
        /// 随机金额或者数量
        /// </summary>
        Random = 1
    }
}
