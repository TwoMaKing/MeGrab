using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace MeGrab.Domain.Models
{
    /// <summary>
    /// 派发模式
    /// </summary>
    [Serializable()]
    [DataContract()]
    public enum DispatchMode
    {
        /// <summary>
        /// 固定金额或者数量
        /// </summary>
        [DataMember()]
        [XmlEnum()]
        Fixed = 0,

        /// <summary>
        /// 随机金额或者数量
        /// </summary>
        [DataMember()]
        [XmlEnum()]
        Random = 1
    }
}
