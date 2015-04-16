﻿using Eagle.Domain;
using System;
using System.Collections.Generic;

namespace MeGrab.Domain.Models
{
    /// <summary>
    /// 抢购活动
    /// </summary>
    public abstract class GrabActivity<TGiveaway> : EntityBase<Guid>, IAggregateRoot<Guid>
        where TGiveaway : Giveaway, new()
    {
        private List<MeGrabUser> members = new List<MeGrabUser>();

        /// <summary>
        /// 消息通告
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 参加的人数限制
        /// </summary>
        public int MemberLimit { get; set; }

        /// <summary>
        /// 派发日期
        /// </summary>
        public DateTime DispatchDateTime { get; set; }

        /// <summary>
        /// 开始抢的日期和时间,如果为Null代表立即开始
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime ExpireDateTime { get; set; }

        /// <summary>
        /// 派发者或者派发的商家
        /// </summary>
        public MeGrabUser Dispatcher { get; set; }

        /// <summary>
        /// 抢活动是否结束
        /// </summary>
        public bool Finished { get; private set; }

        /// <summary>
        /// 抢的物品比如红包, 电影票, 优惠券等.
        /// </summary>
        public IEnumerable<TGiveaway> Giveaways { get; set; }

        /// <summary>
        /// 参加的成员
        /// </summary>
        public IEnumerable<MeGrabUser> Members 
        { 
            get 
            {
                return this.members;
            } 
        }

        public void Join(MeGrabUser member)
        {

        }
    }
}
