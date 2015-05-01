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
        private IRedPacketGrabActivitySqlRepository redPacketGrabActivityRepository;
        private ICacheManager cacheManager;

        public RedPacketQueryServiceImpl(IRepositoryContext repositoryContext, 
                                         IRedPacketGrabActivitySqlRepository redPacketGrabActivityRepository,
                                         ICacheManager cacheManager)
            : base(repositoryContext) 
        {
            this.redPacketGrabActivityRepository = redPacketGrabActivityRepository;
            this.cacheManager = cacheManager;
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetIntradayRedPacketGrabActivities()
        {
            return null;
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByStartDateTime(DateTime startDateTime)
        {
            string cacheKey = "RedPacketGrabActivities_Query_By_StartDateTime:" + startDateTime.ToString("yyyy-MM-dd");

            return this.cacheManager.Get<RedPacketGrabActivityDataObject>(cacheKey, () =>
                
                {
                    ISqlCriteriaExpression expression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();

                    expression.EndsWith("rpga_start_datetime", startDateTime.ToString("yyyy-MM-dd"));
                    IEnumerable<RedPacketGrabActivity> redPacketGrabActivities = redPacketGrabActivityRepository.FindAll(expression);

                    ObjectsMapper<RedPacketGrabActivity, RedPacketGrabActivityDataObject> mapper =
                        ObjectMapperManager.DefaultInstance.GetMapper<RedPacketGrabActivity, RedPacketGrabActivityDataObject>();

                    return mapper.MapEnum(redPacketGrabActivities);
                }, 
                
                3600);
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByExpireDateTime(DateTime expireDateTime)
        {

            throw new NotImplementedException();
        }

        public IPagingResult<RedPacketGrabActivityDataObject> GetRedPacketGrabActivities(int pageNumber, int pageSize)
        {
            string cacheKey = "RedPacketGrabActivities_Paging_By_NA_PageNo:" + pageNumber.ToString() + "PageSize:" + pageSize.ToString();

            return this.cacheManager.Get<PagingResult<RedPacketGrabActivityDataObject>>(cacheKey, () =>
            {
                ISqlCriteriaExpression expression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();

                IPagingResult<RedPacketGrabActivity> pagedRedPacketGrabActivities = redPacketGrabActivityRepository.FindAll(expression, pageNumber, pageSize);

                ObjectsMapper<RedPacketGrabActivity, RedPacketGrabActivityDataObject> mapper =
                    ObjectMapperManager.DefaultInstance.GetMapper<RedPacketGrabActivity, RedPacketGrabActivityDataObject>();

                IEnumerable<RedPacketGrabActivityDataObject> redPacketGrabActivityDataObjectList = 
                    mapper.MapEnum(pagedRedPacketGrabActivities.Data);

                return new PagingResult<RedPacketGrabActivityDataObject>(pagedRedPacketGrabActivities.TotalRecords,
                                                                         pagedRedPacketGrabActivities.TotalPages,
                                                                         pageNumber, 
                                                                         pageSize,
                                                                         redPacketGrabActivityDataObjectList);
            },

            3600);
        }

        public RecentlyDispatchedRedPacketActivityResponse GetRecentlyDispatchedRedPacketGrabActivities(DateTime displayedLastDispatchDateTime)
        {
            using (ICacheProvider cacheManager = CacheProviderFactory.GetCacheProvider())
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
