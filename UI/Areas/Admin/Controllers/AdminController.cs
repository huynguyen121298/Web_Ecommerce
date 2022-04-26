using Model.DTO.DTO_Client;
using Model.DTO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        public ActionResult Index()
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/GetDateRevenue");
            responseMessage.EnsureSuccessStatusCode();
            double? dTO_Accounts = responseMessage.Content.ReadAsAsync<double>().Result;
            ViewData["SaleDate"] = dTO_Accounts;
            return View();
        }

        public ActionResult MonthlySalesByDate()
        {
            var monthlySalesByDate = Request.Form["saleByDate"];

            //check if year is null
            if (monthlySalesByDate == null)
            {
                ViewData["MonthlySalesByDate"] = ("Vui lòng chọn thời gian");
                return View("~/Admin/Admin/Index");
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
    }
}
