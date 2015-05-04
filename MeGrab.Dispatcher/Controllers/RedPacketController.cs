using Eagle.Core;
using Eagle.Core.Generators;
using MeGrab.Application;
using MeGrab.DataObjects;
using MeGrab.Dispatcher.Filters;
using MeGrab.Dispatcher.Models;
using MeGrab.Domain;
using MeGrab.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeGrab.Dispatcher.Controllers
{
    public class RedPacketController : Controller
    {
        //
        // GET: /RedPacket/

        public ActionResult Index()
        {
            RedPacketGrabActivityModel model = new RedPacketGrabActivityModel();
            return View("ActivityView", model);
        }

        public ActionResult RedPacketGrabActivitiesByStartDateTime(DateTime startDateTime)
        {
            using(IRedPacketGrabActivityQueryService queryService = ServiceLocator.Instance.GetService<IRedPacketGrabActivityQueryService>())
            {
                IEnumerable<RedPacketGrabActivityDataObject> activityDataObjectList = 
                    queryService.GetRedPacketGrabActivitiesByStartDateTime(startDateTime);

                RedPacketGrabActivityModel model = new RedPacketGrabActivityModel();
                model.StartDateTime = startDateTime;
                model.RedPacketGrabActivityList = activityDataObjectList;

                return View("ActivityView", model);
            }
        }
    }
}
