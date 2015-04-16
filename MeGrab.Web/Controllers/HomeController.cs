using MeGrab.DataObjects;
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
    [SSOAuthorize("http://localhost:8800/Home/Index", new string[] { "SnatchRedPacket" })]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SnatchRedPacket(string redPacketActivityId)
        {
            return View("Index");
        }

    }
}
