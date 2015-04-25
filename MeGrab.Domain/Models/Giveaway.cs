using Eagle.Domain;
using System;

namespace MeGrab.Domain.Models
{
    /// <summary>
    /// 代表用于秒杀/赠送/抢购物品的基类
    /// </summary>
    [Serializable()]
    public abstract class Giveaway : EntityBase<Guid>
    {
        public bool Snatched 
        {
            get 
            {
                return this.Snatcher != null &&
                       this.SnatchDateTime != null; 
            }
        }

        public MeGrabUser Snatcher { get; set; }

        public DateTime? SnatchDateTime { get; set; }
    }
}
