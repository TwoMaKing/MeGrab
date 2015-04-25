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

namespace MeGrab.RedPacketActivity.Storage.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [ActionName("IndexData")]
        public ActionResult Index(string guid, decimal totalAmount, DateTime startDateTime, DateTime expireDateTime)
        {
            ViewBag.Guid = guid;
            ViewBag.TotalAmount = totalAmount;
            ViewBag.StartDateTime = startDateTime;
            ViewBag.ExpireDateTime = expireDateTime;

            return View("Index");
        }

    }
}
