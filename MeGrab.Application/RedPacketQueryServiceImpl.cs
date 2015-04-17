using Eagle.Core;
using Eagle.Domain;
using Eagle.Domain.Application;
using Eagle.Domain.Repositories;
using Eagle.Web.Caches;
using EmitMapper;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
using MeGrab.ServiceContracts;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Application
{
    public class RedPacketQueryServiceImpl : DisposableObject, IRedPacketQueryService
    {
        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivities(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public RecentlyDispatchedRedPacketActivityResponse GetRecentlyDispatchedRedPacketGrabActivities(DateTime displayedLastDispatchDateTime)
        {
            using (ICacheManager cacheManager = CacheFactory.GetCacheManager())
            {
                using (RedisClient redisClient = cacheManager.GetCacheProvider<RedisClient>())
                {
                    IRedisTypedClient<RedPacketGrabActivity> redPacketRedisClient = redisClient.As<RedPacketGrabActivity>();

                    List<RedPacketGrabActivity> redPacketGrabActivityList =
                        redPacketRedisClient.GetAllItemsFromSortedSet(redPacketRedisClient.SortedSets["Dispatch_InMemory_RedPacketGrabActivity"]);

                    IEnumerable<RedPacketGrabActivity> redPacketGrabActivitiesToDisplay = redPacketGrabActivityList.Where(r => r.DispatchDateTime > DateTime.Now);

                    List<RedPacketGrabActivityDataObject> activityDataObjectList = new List<RedPacketGrabActivityDataObject>();

                    if (redPacketGrabActivitiesToDisplay.Count() > 0)
                    {
                        ObjectsMapper<RedPacketGrabActivity, RedPacketGrabActivityDataObject> mapper =
                        ObjectMapperManager.DefaultInstance.GetMapper<RedPacketGrabActivity, RedPacketGrabActivityDataObject>();

                        foreach (RedPacketGrabActivity activityItem in redPacketGrabActivitiesToDisplay)
                        {
                            RedPacketGrabActivityDataObject activityDataObject = mapper.Map(activityItem);
                            activityDataObjectList.Add(activityDataObject);
                        }
                    }

                    RecentlyDispatchedRedPacketActivityResponse response = new RecentlyDispatchedRedPacketActivityResponse();
                    response.RedPacketGrabActivities = activityDataObjectList;
                    if (activityDataObjectList.Count > 0)
                    {
                        response.LastDispatchedDateTime = activityDataObjectList[activityDataObjectList.Count - 1].DispatchDateTime;
                    }

                    return response;
                }
            }
        }

        protected override void Dispose(bool disposing) { }

    }
}
