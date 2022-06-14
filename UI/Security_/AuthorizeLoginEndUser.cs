using UI.Models;
using Model.Common;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UI.Security_
{
    public class AuthorizeLoginEndUser : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var session = (UserLogin)HttpContext.Current.Session[Constants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Customer", action = "Login" }));
            }

        }
    }
}