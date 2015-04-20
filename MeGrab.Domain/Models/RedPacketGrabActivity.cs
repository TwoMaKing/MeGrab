using Eagle.Core;
using Eagle.Core.Generators;
using Eagle.Domain;
using MeGrab.Domain.Events;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Models
{
    [Alias("RedPacket_Activity")]
    [TablePrimaryKey("Id", ColumnName = "rpga_id", AutoIncrement = false)]
    [TableColumnMappings(new string[] { 
     "MemberLimit=rpga_limit_member", "StartDateTime=rpga_start_datetime", "ExpireDateTime=rpga_expire_datetime", 
     "Message=rpga_message", "DispatcherId=rpga_dispatcher_id", "DispatchDateTime=rpga_dispatch_datetime", 
     "Cancelled=rpga_cancelled", "Finished=rpga_finished", "LastModifiedUserId=rpga_last_modified_user_id" })]
    public class RedPacketGrabActivity : GrabActivity<RedPacket>
    {
        public RedPacketGrabActivity() { }

        public RedPacketGrabActivity(int redPacketCount, decimal totalAmount) :
            this(redPacketCount, totalAmount, DispatchMode.Fixed) { }

        public RedPacketGrabActivity(int redPacketCount, decimal totalAmount, DispatchMode dispatchMode) :
            this(redPacketCount, totalAmount, dispatchMode, string.Empty) { }

        public RedPacketGrabActivity(int redPacketCount, decimal totalAmount, DispatchMode dispatchMode, string message)
        {
            this.RedPacketCount = redPacketCount;
            this.TotalAmount = totalAmount;
            this.Mode = DispatchMode.Fixed;
            this.Message = message;
        }

        [Alias("rpga_redpacket_count")]
        public int RedPacketCount { get; set; }

        [Alias("rpga_total_amount")]
        public decimal TotalAmount { get; set; }

        [Alias("rpga_play_mode")]
        public DispatchMode Mode { get; set; }

        protected override void GenerateGiveaways()
        {
            if (this.Generated)
            {
                return;
            }

            if (this.Mode == DispatchMode.Fixed)
            {

            }
            else if (this.Mode == DispatchMode.Random)
            {

            }
        }

        protected override void DispatchCore(MeGrabUser dispatcher)
        {
            this.Id = (Guid)SequenceGenerator.Instance.Next;
            this.DispatcherId = dispatcher.Id;
            this.DispatchDateTime = DateTime.UtcNow;

            // 通过领域事件
            // 放入缓存
            // 进入 保存队列
            RedPacketGrabActivityEvent @event = new RedPacketGrabActivityEvent(this);
            this.RaiseEvent<RedPacketGrabActivityEvent>(@event);
        }

    }
}
