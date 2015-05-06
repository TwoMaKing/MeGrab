using Eagle.Domain.Application;
using EmitMapper;
using MeGrab.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MeGrab.DataObjects
{
    /// <summary>
    /// 代表抢红包活动的数据传输对象
    /// </summary>
    [DataContract()]
    public class RedPacketGrabActivityDataObject : DataTransferObjectBase<RedPacketGrabActivity, Guid>
    {
        /// <summary>
        /// 红包个数
        /// </summary>
        [DataMember()]
        public int RedPacketCount { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [DataMember()]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 留言通告
        /// </summary>
        [DataMember()]
        public string Message { get; set; }

        /// <summary>
        /// 参加的人数限制
        /// </summary>
        [DataMember()]
        public int MemberLimit { get; set; }

        /// <summary>
        /// 派发日期
        /// </summary>
        [DataMember()]
        public DateTime DispatchDateTime { get; set; }

        /// <summary>
        /// 开始抢的日期和时间,如果为Null代表立即开始
        /// </summary>
        [DataMember()]
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [DataMember()]
        public DateTime ExpireDateTime { get; set; }

        /// <summary>
        /// 派发者或者派发的商家
        /// </summary>
        [DataMember()]
        public MeGrabUserDataObject Dispatcher { get; set; }

        /// <summary>
        /// 抢活动是否结束
        /// </summary>
        [DataMember()]
        public bool Finished { get; private set; }

        /// <summary>
        /// 派发模式
        /// </summary>
        [DataMember()]
        public DispatchMode DispatchMode { get; set; }

        /// <summary>
        /// 包含的红包信息
        /// </summary>
        [DataMember()]
        public IEnumerable<RedPacketDataObject> RedPackets { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            RedPacketGrabActivityDataObject other = obj as RedPacketGrabActivityDataObject;
            if (other == ((RedPacketGrabActivityDataObject)null))
            {
                return false;
            }

            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        protected override void DoMapFrom(RedPacketGrabActivity domainModel)
        {

        }

        protected override RedPacketGrabActivity DoMapTo()
        {
            ObjectsMapper<RedPacketGrabActivityDataObject, RedPacketGrabActivity> mapper =
                    ObjectMapperManager.DefaultInstance.GetMapper<RedPacketGrabActivityDataObject, RedPacketGrabActivity>();

            return mapper.Map(this);
        }
    }
}
