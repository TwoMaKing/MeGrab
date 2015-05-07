using Eagle.Common.Util;
using Eagle.Core;
using Eagle.Core.Extensions;
using Eagle.Core.Query;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
using MeGrab.ServiceContracts;
using MeGrab.Web.Filters;
using MeGrab.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MeGrab.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.RedPacketGrabActivitiesByPaging(1, 10);
        }

        public ActionResult RedPacketGrabActivitiesByStartDateTime(DateTime startDateTime)
        {
            using (IRedPacketGrabActivityQueryService queryService = ServiceLocator.Instance.GetService<IRedPacketGrabActivityQueryService>())
            {
                IEnumerable<RedPacketGrabActivityDataObject> activityDataObjectList =
                    queryService.GetRedPacketGrabActivitiesByStartDateTime(startDateTime);

                RedPacketGrabActivityModel model = new RedPacketGrabActivityModel();
                model.StartDateTime = startDateTime;
                model.RedPacketGrabActivityList = activityDataObjectList;

                return View("Index", model);
            }
        }

        public ActionResult RedPacketGrabActivitiesByPaging(int pageNo, int pageSize)
        {
            using (IRedPacketGrabActivityQueryService queryService = ServiceLocator.Instance.GetService<IRedPacketGrabActivityQueryService>())
            {
                IPagingResult<RedPacketGrabActivityDataObject> pagedActivityDataObjects =
                    queryService.GetRedPacketGrabActivities(pageNo, pageSize);

                RedPacketGrabActivityQueriesModel model = new RedPacketGrabActivityQueriesModel();
                model.PagingRedPacketGrabActivities = pagedActivityDataObjects.Data;
                model.TotalRecords = pagedActivityDataObjects.TotalRecords;
                model.TotalPages = pagedActivityDataObjects.TotalPages;
                model.PageNo = pagedActivityDataObjects.PageNumber.Value;
                model.PageSize = pagedActivityDataObjects.PageSize.Value;

                return View("Index", model);
            }
        }

        public ActionResult RedPacketGrabActivityList(DateTime startDateTime, 
                                                      DateTime? expireDateTime, 
                                                      string totalAmountRange,
                                                      DispatchMode? playMode, 
                                                      int pageNumber, 
                                                      int pageSize)
        {
            using (IRedPacketGrabActivityQueryService queryService = ServiceLocator.Instance.GetService<IRedPacketGrabActivityQueryService>())
            {
                RedPacketGrabActivityQueryServiceRequest queryRequest = new RedPacketGrabActivityQueryServiceRequest();
                
                decimal?[] selectedTotalAmountRange = new decimal?[2];

                queryRequest.StartDateTimeRange = new DateTimeCriteriaRange();
                queryRequest.StartDateTimeRange.FromDateTime = startDateTime;
                queryRequest.StartDateTimeRange.ToDateTime = startDateTime;

                queryRequest.ExpireDateTimeRange = new DateTimeCriteriaRange();
                queryRequest.ExpireDateTimeRange.FromDateTime = expireDateTime;
                queryRequest.ExpireDateTimeRange.ToDateTime = expireDateTime;

                if (totalAmountRange.HasValue())
                {
                    string[] splittedTotalAmountRange = totalAmountRange.Split('-');
                    queryRequest.TotalAmountRange = new TotalAmountCriteriaRange();
                    
                    queryRequest.TotalAmountRange.FromTotalAmount = LocalizationUtils.FormatStringTo2Decimal(splittedTotalAmountRange[0]);
                    queryRequest.TotalAmountRange.ToTotalAmount = LocalizationUtils.FormatStringTo2Decimal(splittedTotalAmountRange[1]);

                    selectedTotalAmountRange[0] = queryRequest.TotalAmountRange.FromTotalAmount;
                    selectedTotalAmountRange[1] = queryRequest.TotalAmountRange.ToTotalAmount;
                }

                queryRequest.DispatchMode = playMode;
                queryRequest.PageNumber = pageNumber;
                queryRequest.PageSize = pageSize;

                RedPacketGrabActivityQueryServiceResponse ActivityQueryResponse =
                    queryService.GetRedPacketGrabActivitiesByQueryServiceRequest(queryRequest);

                IPagingResult<RedPacketGrabActivityDataObject> pagedActivityDataObjects = 
                    ActivityQueryResponse.RedPacketGrabActivities;

                RedPacketGrabActivityQueriesModel model = new RedPacketGrabActivityQueriesModel();

                model.SelectedStartDateTime = startDateTime;
                model.SelectedExpireDateTime = expireDateTime;
                model.SelectedTotalAmountRange = selectedTotalAmountRange;
                model.SelectedPlayModel = playMode;

                model.PagingRedPacketGrabActivities = pagedActivityDataObjects.Data;
                model.TotalRecords = pagedActivityDataObjects.TotalRecords;
                model.TotalPages = pagedActivityDataObjects.TotalPages;
                model.PageNo = pagedActivityDataObjects.PageNumber.Value;
                model.PageSize = pagedActivityDataObjects.PageSize.Value;

                return View("Index", model);
            }
        }

        [HttpPost()]
        public ActionResult Join(Guid activityId)
        {
            using (IRedPacketGrabActivityCommandService commandService = ServiceLocator.Instance.GetService<IRedPacketGrabActivityCommandService>())
            {       
                IEnumerable<MeGrabUserDataObject> participants = commandService.Join(activityId, 5);

                return new JsonResult() { Data = participants };
            }
        }

    }
}
