using Eagle.Core;
using MeGrab.Application;
using MeGrab.DataObjects;
using MeGrab.Domain;
using MeGrab.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeGrab.Dispatcher.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /RedPacket/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dispatch(RedPacketGrabActivityDataObject redPacketGrabActivity)
        {
            using (IRedPacketDispatchService redPacketDispatchService = ServiceLocator.Instance.GetService<IRedPacketDispatchService>()) 
            {
                DispatchRequest dispatchRequest = new DispatchRequest();
                dispatchRequest.RedPacketGrabActivity = redPacketGrabActivity;
                redPacketDispatchService.Dispatch(dispatchRequest);
            }

            return View();
        }
    }
}
