using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.DataObjects
{
    /// <summary>
    /// 派发请求 
    /// </summary>
    [Serializable()]
    [DataContract()]
    public class DispatchRequest
    {
        [DataMember()]
        public string DispatcherName { get; set; }

        /// <summary>
        /// 大红包信息, 聚合了N个小红包，每个小红包有金额，领取人，领取时间等字段。 
        /// 派发的时候把N个小红包都放置到未领取的红包消息队列
        /// </summary>
        [DataMember()]
        public RedPacketGrabActivityDataObject RedPacketGrabActivity { get; set; }
    }
}
