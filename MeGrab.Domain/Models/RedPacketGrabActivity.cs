using Eagle.Core;
using Eagle.Domain;
using Eagle.MessageQueue;
using Eagle.MessageQueue.Redis;
using Eagle.Web.Caches;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Domain.Models
{
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

        public int RedPacketCount { get; set; }

        public decimal TotalAmount { get; set; }

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

        protected override void DispatchCore()
        {
            //// 进入 显示在首页 队列
            //using (IMessageQueueBus<RedPacketGrabActivity> showingMQBus = new DistributedRedisMQBus<RedPacketGrabActivity>("MQ.ShowingRedPacketActivity"))
            //{
            //    showingMQBus.Publish(this);
            //    showingMQBus.Commit();
            //}

            //放入缓存
            using (ICacheManager cacheManager = CacheFactory.GetCacheManager())
            {
                using (RedisClient redisClient = cacheManager.GetCacheProvider<RedisClient>())
                {
                    IRedisTypedClient<RedPacketGrabActivity> redPacketRedisClient = redisClient.As<RedPacketGrabActivity>();
                    redPacketRedisClient.AddItemToSortedSet(redPacketRedisClient.SortedSets["Dispatch_InMemory_RedPacketGrabActivity"], this);
                }
            }

            // 进入 保存队列
            using (IMessageQueueBus<RedPacketGrabActivity> storingMQBus = new DistributedRedisMQBus<RedPacketGrabActivity>("MQ.StoringRedPacketActivity"))
            {
                storingMQBus.Publish(this);
                storingMQBus.Commit();
            }
        }

    }
}
