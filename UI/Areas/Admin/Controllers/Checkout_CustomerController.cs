using Model.DTO.DTO_Ad;
using Model.DTO_Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using UI.Areas.Admin.Common;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class Checkout_CustomerController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        public ActionResult Index(string seachby)
        {
            var timkiemtim = Request.Form["timkiemtim"];

            if (seachby == "id")
            {
                HttpResponseMessage responseMessage1 = service.GetResponse("api/Checkout_Customer/GetListcustomerById/" + timkiemtim);
                responseMessage1.EnsureSuccessStatusCode();
                List<DTOCheckoutCustomerOrder> dtocustomer = responseMessage1.Content.ReadAsAsync<List<DTOCheckoutCustomerOrder>>().Result;
                return View(dtocustomer);
            }
            else
            {
                DTO_Account dTO_Account = new DTO_Account();
                dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
                HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/getallcustomer/"+dTO_Account.AccountId);
                responseMessage.EnsureSuccessStatusCode();
                List<DTOCheckoutCustomerOrder> DTO_Checkout_Customers = responseMessage.Content.ReadAsAsync<List<DTOCheckoutCustomerOrder>>().Result;
                return View(DTO_Checkout_Customers);
            }
        }

        public ActionResult Edit(string id)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/GetcustomerById/" + id);
            responseMessage.EnsureSuccessStatusCode();
            DTOCheckoutCustomerOrder dtocustomer = responseMessage.Content.ReadAsAsync<DTOCheckoutCustomerOrder>().Result;

            return View(dtocustomer);
        }

        public ActionResult Details(string id)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/GetCustomerById/" + id);
            responseMessage.EnsureSuccessStatusCode();
            DTOCheckoutCustomerOrder dtocustomer = responseMessage.Content.ReadAsAsync<DTOCheckoutCustomerOrder>().Result;

            return View(dtocustomer);
        }

        [HttpPost]
        public ActionResult Edit(DTOCheckoutCustomerOrder DTO_Checkout_Customer)
        {
            DTO_Checkout_Customer.TrangThai = Request.Form["stt"];
            HttpResponseMessage response = service.PostResponse("api/Checkout_Customer/Update/", DTO_Checkout_Customer);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            try
            {
                    HttpResponseMessage response = service.DeleteResponse("api/Checkout_Customer/Deletecustomer/" + id);
                    response.EnsureSuccessStatusCode();

                    return Json(new { mes = true });
            }
            catch
            {
                ViewBag.Mess = "Có lỗi ngoài ý muốn, vui lòng kiểm tra lại";
                return Json(new { mes = false });
            }
        }
    }
}
