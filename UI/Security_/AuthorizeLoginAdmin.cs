using Model.DTO.DTO_Ad;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UI.Areas.Admin.Common;

namespace UI.Security_
{
    public class AuthorizeLoginAdmin : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
           
            var session = (DTO_Account)HttpContext.Current.Session[CommonConstants.ACCOUNT_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "Admin" }));
            }
           
        }
    }
}