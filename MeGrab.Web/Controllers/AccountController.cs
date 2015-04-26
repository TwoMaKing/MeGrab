using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Domain.Repositories;
using Eagle.Web.Security;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using MeGrab.ServiceContracts;
using MeGrab.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeGrab.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Register/

        public ActionResult Index()
        {
            MeGrabUserDataObject user = new MeGrabUserDataObject();

            return View(user);
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        public ActionResult Register(MeGrabUserDataObject user)
        {
            throw new NotImplementedException();
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (RdbmsWebSecurity.Login(loginModel.UserName, loginModel.Password, true) != null)
                {
                    if (!string.IsNullOrEmpty(returnUrl) &&
                        Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(loginModel);
        }

    }
}
