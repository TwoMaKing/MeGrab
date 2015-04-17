using Eagle.Domain.Application;
using MeGrab.DataObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace MeGrab.ServiceContracts
{
    [ServiceContract]
    public interface IRedPacketQueryService : IApplicationServiceContract
    {
        [OperationContract()]
        IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivities(int pageNumber, int pageSize);
        
        [OperationContract()]
        RecentlyDispatchedRedPacketActivityResponse GetRecentlyDispatchedRedPacketGrabActivities(DateTime displayedLastDispatchDateTime);
    }
}
