using Eagle.Core;
using Eagle.Core.Query;
using Eagle.Core.SqlQueries;
using Eagle.Core.SqlQueries.Criterias;
using Eagle.Core.SqlQueries.DialectProvider;
using Eagle.Domain;
using Eagle.Domain.Application;
using Eagle.Domain.Repositories;
using Eagle.Web.Caches;
using EmitMapper;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
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
    public class RedPacketQueryServiceImpl : ApplicationService, IRedPacketQueryService
    {
        private IRedPacketGrabActivityRepository redPacketGrabActivityRepository;
        private IRedPacketGrabActivitySqlRepository redPacketGrabActivitySqlRepository;
        
        public RedPacketQueryServiceImpl(IRepositoryContext repositoryContext, 
                                         IRedPacketGrabActivityRepository redPacketGrabActivityRepository,
                                         IRedPacketGrabActivitySqlRepository redPacketGrabActivitySqlRepository) : base(repositoryContext) 
        {
            this.redPacketGrabActivityRepository = redPacketGrabActivityRepository;
            this.redPacketGrabActivitySqlRepository = redPacketGrabActivitySqlRepository;
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetIntradayRedPacketGrabActivities()
        {
            return null;
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByStartDateTime(DateTime startDateTime)
        {
            ISqlCriteriaExpression expression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();

            expression.Equals("rpga_start_datetime", startDateTime);
            IEnumerable<RedPacketGrabActivity> redPacketGrabActivities = redPacketGrabActivitySqlRepository.FindAll(expression);

            ObjectsMapper<RedPacketGrabActivity, RedPacketGrabActivityDataObject> mapper =
                ObjectMapperManager.DefaultInstance.GetMapper<RedPacketGrabActivity, RedPacketGrabActivityDataObject>();
                
            return mapper.MapEnum(redPacketGrabActivities);
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByExpireDateTime(DateTime expireDateTime)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivities(int pageNumber, int pageSize)
        {
            return null;
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

    }
}
