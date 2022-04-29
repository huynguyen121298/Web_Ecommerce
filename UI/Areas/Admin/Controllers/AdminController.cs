using Model.DTO.DTO_Ad;
using Model.DTO.DTO_Client;
using Model.DTO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using UI.Areas.Admin.Common;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        [AuthorizeLoginAdmin]
        public ActionResult Index()
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/GetDateRevenue");
            responseMessage.EnsureSuccessStatusCode();
            double? dTO_Accounts = responseMessage.Content.ReadAsAsync<double>().Result;
            ViewData["SaleDate"] = dTO_Accounts;
            return View();
        }

        [AuthorizeLoginAdmin]
        public ActionResult MonthlySalesByDate(FormCollection formCollection)
        {
            string monthlySalesByDate = formCollection["saleByDate2"];
            //check if year is null
            if (monthlySalesByDate == "")
            {
                ViewData["MonthlySalesByDate"] = ("Vui lòng chọn thời gian");
                return RedirectToAction("Index", "Admin");
            }

            HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/GetMonthlyRevenue/"+monthlySalesByDate);
            responseMessage.EnsureSuccessStatusCode();
            DtoSalesVM dTO_Accounts = responseMessage.Content.ReadAsAsync<DtoSalesVM>().Result;
            return View(dTO_Accounts);


        }

        [DeatAuthorize(Order = 2)]
        public ActionResult GetAllFeedbacks()
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Home/getallfeedback");
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Feedback> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Feedback>>().Result;
            return View(dTO_Accounts);
        }

        public PartialViewResult BagNotification()
        {

            try
            {
                var dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
                
                HttpResponseMessage responseUser = service.GetResponse("api/notification/GetNotiByMerchant/" + dTO_Account._id);

                responseUser.EnsureSuccessStatusCode();
                List<DtoMerchantNotification> result = responseUser.Content.ReadAsAsync<List<DtoMerchantNotification>>().Result;

               
                ViewBag.Quantity = result;

                
            }
            catch
            {

                ViewBag.Quantity = 0;

            }
            return PartialView("BagNotification");
        }
    }
}
