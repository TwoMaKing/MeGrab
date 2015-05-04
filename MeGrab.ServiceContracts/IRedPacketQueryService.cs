using Eagle.Core.Query;
using Eagle.Domain.Application;
using MeGrab.DataObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace MeGrab.ServiceContracts
{
    [ServiceContract]
    public interface IRedPacketGrabActivityQueryService : IApplicationServiceContract
    {
        /// <summary>
        /// 得到指定发布日期那天发布的抢红包活动
        /// </summary>
        /// <returns>The instance of IEnumerable<RedPacketGrabActivityDataObject></returns>
        [OperationContract()]
        IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByDispatchDateTime(DateTime dispatchDateTime);

        /// <summary>
        /// 得到指定的开始日期那天的抢红包活动
        /// </summary>
        /// <param name="startDateTime">开始日期</param>
        /// <returns>The instance of IEnumerable<RedPacketGrabActivityDataObject></returns>
        [OperationContract()]
        IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByStartDateTime(DateTime startDateTime);

        /// <summary>
        /// 得到指定的结束日期那天的抢红包活动
        /// </summary>
        /// <param name="startDateTime">结束日期</param>
        /// <returns>The instance of IEnumerable<RedPacketGrabActivityDataObject></returns>
        [OperationContract()]
        IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByExpireDateTime(DateTime expireDateTime);

        /// <summary>
        /// 得到指定的分页抢红包活动
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [OperationContract()]
        IPagingResult<RedPacketGrabActivityDataObject> GetRedPacketGrabActivities(int pageNumber, int pageSize);

        /// <summary>
        /// 根据 queryServiceRequest 得到 指定红包活动的 QueryServiceResponse
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [OperationContract()]
        RedPacketGrabActivityQueryServiceResponse GetRedPacketGrabActivitiesByQueryServiceRequest
                                                  (RedPacketGrabActivityQueryServiceRequest queryServiceRequest);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayedLastDispatchDateTime"></param>
        /// <returns></returns>
        [OperationContract()]
        RecentlyDispatchedRedPacketActivityResponse GetRecentlyDispatchedRedPacketGrabActivities(DateTime displayedLastDispatchDateTime);

    }
}
