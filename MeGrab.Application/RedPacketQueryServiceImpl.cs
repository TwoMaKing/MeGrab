using Eagle.Common.Util;
using Eagle.Core;
using Eagle.Core.Extensions;
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
    public class RedPacketGrabActivityQueryServiceImpl : ApplicationService, IRedPacketGrabActivityQueryService
    {
        private IRedPacketGrabActivitySqlRepository redPacketGrabActivityRepository;
        private ICacheManager cacheManager;

        public RedPacketGrabActivityQueryServiceImpl(IRepositoryContext repositoryContext, 
                                                     IRedPacketGrabActivitySqlRepository redPacketGrabActivityRepository,
                                                     ICacheManager cacheManager)
            : base(repositoryContext) 
        {
            this.redPacketGrabActivityRepository = redPacketGrabActivityRepository;
            this.cacheManager = cacheManager;
        }

        private ISqlCriteriaExpression GetRedPacketGrabActivitiesSqlCriteriaExpression(RedPacketGrabActivityQueryServiceRequest queryServiceRequest, out string cacheKey)
        {
            StringBuilder cacheKeyBuilder = new StringBuilder();

            ISqlCriteriaExpression expression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();

            if (queryServiceRequest.StartDateTimeRange != null)
            {
                bool havingFromStartDateTime = queryServiceRequest.StartDateTimeRange.FromDateTime.HasValue;
                bool havingToStartDateTime = queryServiceRequest.StartDateTimeRange.FromDateTime.HasValue;

                string fromStartDateTime = DateTimeUtils.ToDateTimeString(queryServiceRequest.StartDateTimeRange.FromDateTime, "yyyy-MM-dd");
                string toStartDateTime = DateTimeUtils.ToDateTimeString(queryServiceRequest.StartDateTimeRange.ToDateTime, "yyyy-MM-dd");

                if (havingFromStartDateTime ||
                    havingToStartDateTime)
                {
                    if (havingFromStartDateTime &&
                        havingToStartDateTime &&
                        fromStartDateTime.Equals(toStartDateTime))
                    {
                        expression.Equals("rpga_start_datetime", fromStartDateTime, (c) => "date_format(" + c + ", '%Y-%m-%d')");
                        cacheKeyBuilder.Append("startDateTime=" + fromStartDateTime);
                    }
                    else
                    {
                        if (havingFromStartDateTime)
                        {
                            expression.GreaterThanEquals("rpga_start_datetime", fromStartDateTime, (c) => "date_format(" + c + ", '%Y-%m-%d')");
                            cacheKeyBuilder.Append("startDateTime=" + fromStartDateTime);
                        }

                        if (havingToStartDateTime)
                        {
                            expression.LessThanEquals("rpga_start_datetime", toStartDateTime, (c) => "date_format(" + c + ", '%Y-%m-%d')");
                            cacheKeyBuilder.Append("-" + toStartDateTime);
                        }
                        else
                        {
                            cacheKeyBuilder.Append("-");
                        }
                    }
                }
            }

            if (cacheKeyBuilder.Length != 0)
            {
                cacheKeyBuilder.Append("&");
            }
            cacheKeyBuilder.Append("expireDateTime=");

            if (queryServiceRequest.ExpireDateTimeRange != null)
            {
                ISqlCriteriaExpression expireDateTimeSqlExpression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();

                bool havingFromExpireDateTime = queryServiceRequest.ExpireDateTimeRange.FromDateTime.HasValue;
                bool havingToExpireDateTime = queryServiceRequest.ExpireDateTimeRange.FromDateTime.HasValue;

                string fromExpireDateTime = DateTimeUtils.ToDateTimeString(queryServiceRequest.ExpireDateTimeRange.FromDateTime, "yyyy-MM-dd");
                string toExpireDateTime = DateTimeUtils.ToDateTimeString(queryServiceRequest.ExpireDateTimeRange.ToDateTime, "yyyy-MM-dd");

                if (havingFromExpireDateTime ||
                    havingToExpireDateTime)
                {
                    if (havingFromExpireDateTime &&
                        havingToExpireDateTime &&
                        fromExpireDateTime.Equals(toExpireDateTime))
                    {
                        expireDateTimeSqlExpression.Equals("rpga_expire_datetime", fromExpireDateTime, (c) => "date_format(" + c + ", '%Y-%m-%d')");
                        cacheKeyBuilder.Append(fromExpireDateTime);
                    }
                    else
                    {
                        if (havingFromExpireDateTime)
                        {
                            expireDateTimeSqlExpression.GreaterThanEquals("rpga_expire_datetime", fromExpireDateTime, (c) => "date_format(" + c + ", '%Y-%m-%d')");
                            cacheKeyBuilder.Append(fromExpireDateTime);
                        }

                        if (havingToExpireDateTime)
                        {
                            expireDateTimeSqlExpression.LessThanEquals("rpga_expire_datetime", toExpireDateTime, (c) => "date_format(" + c + ", '%Y-%m-%d')");
                            cacheKeyBuilder.Append("-" + toExpireDateTime);
                        }
                        else
                        {
                            cacheKeyBuilder.Append("-");
                        }
                    }

                    expression.And(expireDateTimeSqlExpression);
                }
            }

            if (cacheKeyBuilder.Length != 0)
            {
                cacheKeyBuilder.Append("&");
            }
            cacheKeyBuilder.Append("totalAmount=");

            if (queryServiceRequest.TotalAmountRange != null)
            {
                ISqlCriteriaExpression totalAmountSqlExpression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();

                if (queryServiceRequest.TotalAmountRange.FromTotalAmount.HasValue)
                {
                    decimal fromTotalAmount = queryServiceRequest.TotalAmountRange.FromTotalAmount.Value;
                    totalAmountSqlExpression.GreaterThanEquals("rpga_total_amount", fromTotalAmount);
                    cacheKeyBuilder.Append(fromTotalAmount);
                }

                if (queryServiceRequest.TotalAmountRange.ToTotalAmount.HasValue)
                {
                    decimal toTotalAmount = queryServiceRequest.TotalAmountRange.ToTotalAmount.Value;
                    totalAmountSqlExpression.LessThanEquals("rpga_total_amount", toTotalAmount);
                    cacheKeyBuilder.Append("-" + toTotalAmount);
                }
                else
                {
                    cacheKeyBuilder.Append("-");
                }

                expression.And(totalAmountSqlExpression);
            }

            if (cacheKeyBuilder.Length != 0)
            {
                cacheKeyBuilder.Append("&");
            }
            cacheKeyBuilder.Append("redPacketCount=");

            if (queryServiceRequest.RedPacketCountRange != null)
            {
                ISqlCriteriaExpression redPacketCountSqlExpression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();

                if (queryServiceRequest.RedPacketCountRange.FromRedPacketCount.HasValue)
                {
                    int fromRedPacketCount = queryServiceRequest.RedPacketCountRange.FromRedPacketCount.Value;
                    redPacketCountSqlExpression.GreaterThanEquals("rpga_redpacket_count", fromRedPacketCount);
                    cacheKeyBuilder.Append(fromRedPacketCount);
                }

                if (queryServiceRequest.RedPacketCountRange.ToRedPacketCount.HasValue)
                {
                    int toRedPacketCount = queryServiceRequest.RedPacketCountRange.ToRedPacketCount.Value;
                    redPacketCountSqlExpression.LessThanEquals("rpga_redpacket_count", toRedPacketCount);
                    cacheKeyBuilder.Append("-" + toRedPacketCount);
                }
                else
                {
                    cacheKeyBuilder.Append("-");
                }

                expression.And(redPacketCountSqlExpression);
            }

            if (cacheKeyBuilder.Length != 0)
            {
                cacheKeyBuilder.Append("&");
            }
            cacheKeyBuilder.Append("memberLimit=");

            if (queryServiceRequest.MemberLimitRange != null)
            {
                ISqlCriteriaExpression memberLimitSqlExpression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();

                if (queryServiceRequest.MemberLimitRange.FromMemberLimit.HasValue)
                {
                    int fromMemberLimit = queryServiceRequest.MemberLimitRange.FromMemberLimit.Value;
                    memberLimitSqlExpression.GreaterThanEquals("rpga_limit_member", fromMemberLimit);
                    cacheKeyBuilder.Append(fromMemberLimit);
                }

                if (queryServiceRequest.MemberLimitRange.ToMemberLimit.HasValue)
                {
                    int toMemberLimit = queryServiceRequest.MemberLimitRange.ToMemberLimit.Value;
                    memberLimitSqlExpression.LessThanEquals("rpga_limit_member", toMemberLimit);
                    cacheKeyBuilder.Append("-" + toMemberLimit);
                }
                else
                {
                    cacheKeyBuilder.Append("-");
                }

                expression.And(memberLimitSqlExpression);
            }

            if (cacheKeyBuilder.Length != 0)
            {
                cacheKeyBuilder.Append("&");
            }
            cacheKeyBuilder.Append("playMode=");

            if (queryServiceRequest.DispatchMode.HasValue)
            {
                expression.Equals("rpga_play_mode", queryServiceRequest.DispatchMode.Value);
                cacheKeyBuilder.Append(queryServiceRequest.DispatchMode.Value);
            }

            if (cacheKeyBuilder.Length > 0)
            {
                cacheKeyBuilder.Append("&");
            }

            cacheKeyBuilder.Append("pageNumber=" + queryServiceRequest.PageNumber);
            cacheKeyBuilder.Append("&pageSize=" + queryServiceRequest.PageSize);

            cacheKeyBuilder.Insert(0, "MeGrab_RedPacketGrabActivity_Queries?");

            cacheKey = cacheKeyBuilder.ToString();

            return expression;
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByDispatchDateTime(DateTime dispatchDateTime)
        {
            return null;
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByStartDateTime(DateTime startDateTime)
        {
            string cacheKey = "MeGrab_RedPacketGrabActivity_Query？StartDateTime=" + startDateTime.ToString("yyyy-MM-dd");

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
            string cacheKey = "MeGrab_RedPacketGrabActivity_Query?PageNo=" + pageNumber.ToString() + "&PageSize=" + pageSize.ToString();

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

        public RedPacketGrabActivityQueryServiceResponse GetRedPacketGrabActivitiesByQueryServiceRequest(RedPacketGrabActivityQueryServiceRequest queryServiceRequest)
        {
            string cacheKey = string.Empty;
            ISqlCriteriaExpression expression = this.GetRedPacketGrabActivitiesSqlCriteriaExpression(queryServiceRequest, out cacheKey);

            Func<PagingResult<RedPacketGrabActivityDataObject>> retrieveFunc = () =>
                {
                    IPagingResult<RedPacketGrabActivity> pagedRedPacketGrabActivities = 
                        redPacketGrabActivityRepository.FindAll(expression, 
                                                                queryServiceRequest.PageNumber, 
                                                                queryServiceRequest.PageSize);

                    ObjectsMapper<RedPacketGrabActivity, RedPacketGrabActivityDataObject> mapper =
                        ObjectMapperManager.DefaultInstance.GetMapper<RedPacketGrabActivity, RedPacketGrabActivityDataObject>();

                    IEnumerable<RedPacketGrabActivityDataObject> redPacketGrabActivityDataObjectList =
                        mapper.MapEnum(pagedRedPacketGrabActivities.Data);

                    return new PagingResult<RedPacketGrabActivityDataObject>(pagedRedPacketGrabActivities.TotalRecords,
                                                                             pagedRedPacketGrabActivities.TotalPages,
                                                                             queryServiceRequest.PageNumber,
                                                                             queryServiceRequest.PageSize,
                                                                             redPacketGrabActivityDataObjectList);

                };

            PagingResult<RedPacketGrabActivityDataObject> pagedRedPacketGrabActivityDataObjects =
                this.cacheManager.Get<PagingResult<RedPacketGrabActivityDataObject>>(cacheKey, retrieveFunc, 3600);

            return new RedPacketGrabActivityQueryServiceResponse() 
                       { RedPacketGrabActivities = pagedRedPacketGrabActivityDataObjects };
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
