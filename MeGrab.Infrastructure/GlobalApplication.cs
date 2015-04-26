using Eagle.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace MeGrab.Infrastructure
{
    public static class GlobalApplication
    {
        public static LoginUserInfo CurrentLoginUser 
        {
            get 
            {
                var currentLoginUser = HttpContext.Current.Session[SessionKeys.SessionKey_CurrentLoginUser];

                if (currentLoginUser == null)
                {
                    return null;
                }

                return (LoginUserInfo)currentLoginUser;
            }
        }

    }
}
