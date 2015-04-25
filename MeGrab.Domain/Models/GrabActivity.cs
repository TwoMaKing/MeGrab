using Eagle.Domain;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeGrab.Domain.Models
{
    /// <summary>
    /// 抢购活动
    /// </summary>
    [Serializable()]
    public abstract class GrabActivity<TGiveaway> : AggregateRoot<Guid> //EntityBase<Guid>, IAggregateRoot<Guid>
        where TGiveaway : Giveaway, new()
    {
        private List<MeGrabUser> members = new List<MeGrabUser>();

        /// <summary>
        /// 参加的人数限制
        /// </summary>
        public int MemberLimit { get; set; }

        /// <summary>
        /// 开始抢的日期和时间,如果为Null代表立即开始
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime ExpireDateTime { get; set; }

        /// <summary>
        /// 消息通告
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 派发者或者派发的商家
        /// </summary>
        public int DispatcherId { get; set; }

        /// <summary>
        /// 派发日期
        /// </summary>
        public DateTime DispatchDateTime { get; set; }

        /// <summary>
        /// 活动是否取消
        /// </summary>
        public bool Cancelled { get; set; }

        /// <summary>
        /// 抢活动是否结束
        /// </summary>
        public bool Finished { get; set; }

        /// <summary>
        /// 哪个用户最后一次更新的该活动
        /// </summary>
        public int LastModifiedUserId { get; set; }

        /// <summary>
        /// 最后一次更新日期时间
        /// </summary>
        public DateTime LastModifiedDateTime { get; set; }

        /// <summary>
        /// Giveaways是否已经生成
        /// </summary>
        [Ignore()]
        public bool Generated
        {
            get
            {
                return this.Giveaways != null &&
                       this.Giveaways.Count() > 0;
            }
        }

        /// <summary>
        /// 派发者或者派发的商家
        /// </summary>
        public MeGrabUser Dispatcher { get; set; }

        /// <summary>
        /// 抢的物品比如红包, 电影票, 优惠券等.
        /// </summary>
        [Reference()]
        public IEnumerable<TGiveaway> Giveaways { get; set; }

        /// <summary>
        /// 参加的成员
        /// </summary>
        [Ignore()]
        public IEnumerable<MeGrabUser> Members
        {
            get
            {
                return this.members;
            }
        }

        /// <summary>
        /// 生成 Giveaways
        /// </summary>
        protected abstract void GenerateGiveaways();

        /// <summary>
        /// 发布的核心逻辑
        /// </summary>
        protected abstract void DispatchCore(MeGrabUser dispatcher);

        /// <summary>
        /// 参加活动
        /// </summary>
        public virtual void Join(MeGrabUser member)
        {
            if (!this.members.Exists(m => m.Name == member.Name))
            {
                this.members.Add(member);
            }
        }

        /// <summary>
        /// 取消该活动
        /// </summary>
        public virtual void Cancel()
        {
            this.Cancelled = true;
        }

        public void Dispatch(MeGrabUser dispatcher)
        {
            this.GenerateGiveaways();
            this.DispatchCore(dispatcher);
        }
    }
}
