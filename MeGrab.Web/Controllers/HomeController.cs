using Eagle.Core;
using Eagle.Core.Query;
using MeGrab.DataObjects;
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
            using (IRedPacketQueryService queryService = ServiceLocator.Instance.GetService<IRedPacketQueryService>())
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
            using (IRedPacketQueryService queryService = ServiceLocator.Instance.GetService<IRedPacketQueryService>())
            {
                IPagingResult<RedPacketGrabActivityDataObject> pagedActivityDataObjects =
                    queryService.GetRedPacketGrabActivities(pageNo, pageSize);

                PagingRedPacketGrabActivityModel model = new PagingRedPacketGrabActivityModel();
                model.PagingRedPacketGrabActivities = pagedActivityDataObjects.Data;
                model.TotalRecords = pagedActivityDataObjects.TotalRecords;
                model.TotalPages = pagedActivityDataObjects.TotalPages;
                model.PageNo = pagedActivityDataObjects.PageNumber;
                model.PageSize = pagedActivityDataObjects.PageSize;

                return View("Index", model);
            }
        }

    }
}
