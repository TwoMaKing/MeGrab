using Eagle.Core;
using Eagle.Core.Query;
using Eagle.Core.SqlQueries;
using Eagle.Core.SqlQueries.Criterias;
using MeGrab.Application;
using MeGrab.DataObjects;
using MeGrab.Domain;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using MeGrab.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace MeGrab.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    //[JavascriptCallbackBehavior(UrlParameterName = "jsoncallback")]
    public class RedPacketGabActivityQueryService : IRedPacketGrabActivityQueryService
    {
        private readonly IRedPacketGrabActivityQueryService queryServiceImpl = 
            ServiceLocator.Instance.GetService<IRedPacketGrabActivityQueryService>();

        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByDispatchDateTime(DateTime dispatchDateTime)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByStartDateTime(DateTime startDateTime)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RedPacketGrabActivityDataObject> GetRedPacketGrabActivitiesByExpireDateTime(DateTime expireDateTime)
        {
            throw new NotImplementedException();
        }

        public IPagingResult<RedPacketGrabActivityDataObject> GetRedPacketGrabActivities(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        [WebInvoke(UriTemplate="Search_Activities", Method="Post", RequestFormat = WebMessageFormat.Json, ResponseFormat= WebMessageFormat.Json)]
        public RedPacketGrabActivityQueryServiceResponse GetRedPacketGrabActivitiesByQueryServiceRequest(RedPacketGrabActivityQueryServiceRequest queryServiceRequest)
        {
            try
            {
                return queryServiceImpl.GetRedPacketGrabActivitiesByQueryServiceRequest(queryServiceRequest);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public RecentlyDispatchedRedPacketActivityResponse GetRecentlyDispatchedRedPacketGrabActivities(DateTime displayedLastDispatchDateTime)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
