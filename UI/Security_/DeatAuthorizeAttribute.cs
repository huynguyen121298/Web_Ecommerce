using UI.Areas.Admin.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Model.DTO.DTO_Ad;

namespace UI.Security_
{
    public class DeatAuthorizeAttribute : AuthorizeAttribute
    {
        //1: Member
        //2: Moderator
        //3: Admin
        //4: ...

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Account acc = new Account();
            //DbModels db = new DbModels();
            var session = (DTO_Account)HttpContext.Current.Session[CommonConstants.ACCOUNT_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "Admin" }));
            }
            else
            {
                var hasRole = session.RoleId;
                if (hasRole == Order)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "NotAuthorize" }));
                }
            }
        }
    }
}