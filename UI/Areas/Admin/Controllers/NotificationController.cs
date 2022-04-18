using Model.DTO.DTO_Ad;
using Model.DTO_Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using UI.Areas.Admin.Common;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class NotificationController : Controller
    {
        ServiceRepository service = new ServiceRepository();
        // GET: Admin/Notification
        public ActionResult Index()
        {
            var dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
            HttpResponseMessage responseUser = service.GetResponse("api/notification/GetNotiByMerchant/"+dTO_Account._id);

            responseUser.EnsureSuccessStatusCode();
            List<DtoMerchantNotification> result = responseUser.Content.ReadAsAsync<List<DtoMerchantNotification>>().Result;

            return PartialView(result);
        }
    }
}