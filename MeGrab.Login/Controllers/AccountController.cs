using Eagle.Core.Extensions;
using Eagle.Web.Security;
using MeGrab.Infrastructure;
using MeGrab.Login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MeGrab.Login.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string decodedReturnUrl = this.Server.UrlDecode(returnUrl);

                bool isLocalUrl = !returnUrl.HasValue() || 
                                  Url.IsLocalUrl(decodedReturnUrl);

                LoginUserInfo loginUserInfo = RdbmsWebSecurity.LoginAndCreateSSOToken(model.UserNameOrEmailOrCellPhoneNo, model.Password);

                if (loginUserInfo == null &&
                    !loginUserInfo.PassportToken.HasValue())
                {
                    return View(model);
                }

                // 把当前登陆用户的基本信息 存入 基于Redis 或者 MongoDB 的分布式缓存
                this.Session[SessionKeys.SessionKey_CurrentLoginUser] = loginUserInfo;

                if (isLocalUrl)
                {
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 &&
                        returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && 
                        !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(decodedReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("UserDetails", "Home", new { token = loginUserInfo.PassportToken, 
                                                                             userName = model.UserNameOrEmailOrCellPhoneNo });
                    }
                }
                else
                {
                    string newRedirectedUrl = string.Format("{0}{1}token={2}&username={3}&remark={4}", 
                                                                decodedReturnUrl, 
                                                                "?",
                                                                loginUserInfo.PassportToken, 
                                                                model.UserNameOrEmailOrCellPhoneNo,
                                                                "Success");
                    return Redirect(newRedirectedUrl);
                }
            }

            return View(model);
        }

        public ActionResult SingOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                // Email 或者 手机
                string token = RdbmsWebSecurity.CreateUserAndAccount(model.UserName, model.Password, model.Email);

                if (token.HasValue())
                {
                    if (RdbmsWebSecurity.Login(model.UserName, model.Password, true) != null)
                    {
                        //去到个人信息页面
                        return RedirectToAction("UserDetails", "Home", new { token = token, userName = model.UserName });
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                bool succeeded = true;

                try
                {
                    //MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true);
                    //succeeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    succeeded = false;
                }

                if (succeeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The password is incorrect or new password is invalid.");
                }
            }

            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {

            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Email is invalid. Please enter a different value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please enter a different value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify and try again.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been cancelled. Please verify and try again.";

                default:
                    return "An unknown error occurred.";
            }
        }
        #endregion
    }
}
