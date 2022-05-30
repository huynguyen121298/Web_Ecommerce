using Model.DTO.DTO_Ad;
using Model.DTO_Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using UI.Areas.Admin.Common;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class NotificationController : Controller
    {
        ServiceRepository service = new ServiceRepository();
        // GET: Admin/Notification

        [AuthorizeLoginAdmin]
        public ActionResult Index()
        {
            var dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
            HttpResponseMessage responseUser = service.GetResponse("api/notification/GetNotiByMerchant/"+dTO_Account._id);

            responseUser.EnsureSuccessStatusCode();
            List<DtoMerchantNotification> result = responseUser.Content.ReadAsAsync<List<DtoMerchantNotification>>().Result;

            return PartialView(result);
        }

        [AuthorizeLoginAdmin]
        public ActionResult ChangeStatusNoti(string notiId)
        {
            HttpResponseMessage responseUser = service.GetResponse("api/notification/ChangeStatus/" +notiId );

            DtoMerchantNotification result = responseUser.Content.ReadAsAsync<DtoMerchantNotification>().Result;

            //HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/GetCustomerById/" + result.CheckoutId);
            //responseMessage.EnsureSuccessStatusCode();
            //DTOCheckoutCustomerOrder dtocustomer = responseMessage.Content.ReadAsAsync<DTOCheckoutCustomerOrder>().Result;

            return RedirectToAction("Details", "Checkout_Customer", new { id = result.CheckoutId });
          

            //return Json(new {checkSuccess = true});
        }

        public PartialViewResult BagNotification()
        {

            try
            {
                var dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];

                HttpResponseMessage responseUser = service.GetResponse("api/notification/GetNotiByMerchant/" + dTO_Account._id);

                responseUser.EnsureSuccessStatusCode();
                List<DtoMerchantNotification> result = responseUser.Content.ReadAsAsync<List<DtoMerchantNotification>>().Result;


                ViewBag.Quantity = result.Take(5);
                ViewBag.Count = result.Where(s=>s.Status == 0).Count();
            }
            catch
            {

                ViewBag.Quantity = 0;

            }
            return PartialView("BagNotification");
        }
    }
}