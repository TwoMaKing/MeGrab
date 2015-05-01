using Eagle.Domain;
using Eagle.Domain.Events;
using Eagle.Web.Caches;
using MeGrab.Domain.Models;
using MeGrab.Infrastructure;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Domain.Events.Handlers
{
    public class RedPacketGrabActivityCachingEventHandler : IDomainEventHandler<RedPacketGrabActivityEvent, Guid>
    {
        public void Handle(RedPacketGrabActivityEvent message)
        {
            //放入缓存
            using (ICacheProvider cacheManager = CacheProviderFactory.GetCacheProvider())
            {
                using (RedisClient redisClient = cacheManager.GetCacheProvider<RedisClient>())
                {
                    IRedisTypedClient<RedPacketGrabActivity> redPacketRedisClient = redisClient.As<RedPacketGrabActivity>();

                    redPacketRedisClient.DeleteById("RedisCacheKeys.RedisKey_Intraday_RedPackets");

                    redPacketRedisClient.AddItemToSortedSet(redPacketRedisClient.SortedSets[RedisCacheKeys.RedisKey_Intraday_RedPackets], 
                                                           (RedPacketGrabActivity)message.Source);
                }
            }
        }
    }
}
