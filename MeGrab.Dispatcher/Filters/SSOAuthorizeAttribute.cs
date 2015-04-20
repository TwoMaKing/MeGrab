using Eagle.Core.Extensions;
using Eagle.Web.Security;
using MeGrab.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MeGrab.Dispatcher.Filters
{
    public class SSOAuthorizeAttribute : AuthorizeAttribute
    {
        private string returnUrl;
        private string[] actionsToLoginPage;

        public SSOAuthorizeAttribute(string returnUrl, string[] actionsToLoginPage)
        {
            this.returnUrl = returnUrl;
            this.actionsToLoginPage = actionsToLoginPage;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                string token = filterContext.HttpContext.Request.QueryString["token"];

                if (token.HasValue())
                {
                    FormsIdentity formsIdentity = GetPassportTokenAndFormsIdentity(token);
                    filterContext.HttpContext.User = new GenericPrincipal(formsIdentity, null);
                    FormsAuthentication.SetAuthCookie(formsIdentity.Name, true);
                    return;
                }
                else
                {
                    if (actionsToLoginPage != null &&
                        actionsToLoginPage.Contains(filterContext.ActionDescriptor.ActionName))
                    {
                        string encodedReturnUrl = filterContext.HttpContext.Server.UrlEncode(returnUrl);
                        string loginUrl = FormsAuthentication.LoginUrl + "?returnUrl=" + encodedReturnUrl;
                        filterContext.Result = new RedirectResult(loginUrl);
                        return;
                    }
                }
            }

            return;
        }

        private FormsIdentity GetPassportTokenAndFormsIdentity(string token)
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            using (HttpClient httpClient = new HttpClient(handler))
            {
                string passportServiceUrl = ConfigurationUtils.PassportServiceUrl + token;

                PassportAuthenticationTicket passportAuthTicket = httpClient.GetAsync(passportServiceUrl).Result.
                    Content.ReadAsAsync<PassportAuthenticationTicket>().Result;

                //HttpResponseMessage responseMessage = await httpClient.GetAsync(passportServiceUrl);
                //PassportAuthenticationTicket passportAuthTicket = await responseMessage.Content.ReadAsAsync<PassportAuthenticationTicket>();

                if (passportAuthTicket != null)
                {
                    FormsAuthenticationTicket formsAuthTicket = new FormsAuthenticationTicket(
                        passportAuthTicket.Version,
                        passportAuthTicket.Name,
                        passportAuthTicket.IssueDate,
                        passportAuthTicket.Expiration,
                        passportAuthTicket.IsPersistent,
                        passportAuthTicket.UserData,
                        passportAuthTicket.CookiePath);

                    return new FormsIdentity(formsAuthTicket);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}