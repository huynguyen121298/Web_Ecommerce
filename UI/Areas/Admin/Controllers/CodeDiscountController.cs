using Model.DTO.DTO_Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class CodeDiscountController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        // GET: Admin/CodeDiscount
        [DeatAuthorize(Order = 2)]
        [AuthorizeLoginAdmin]
        public ActionResult Index()
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/CodeDiscount/getall");
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_CodeDiscount> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_CodeDiscount>>().Result;
            return View(dTO_Accounts);
        }

        // GET: Admin/CodeDiscount/Create
        [AuthorizeLoginAdmin]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/CodeDiscount/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection,DTO_CodeDiscount request)
        {
            try
            {
                var discount = double.Parse(collection["SelectDiscount"]);
                request.Discount = discount/100;
                request.Content = collection["SelectDiscount"] + "%";
                HttpResponseMessage responseMessage = service.PostResponse("api/CodeDiscount/create/",request);
                responseMessage.EnsureSuccessStatusCode();
                bool result = responseMessage.Content.ReadAsAsync<bool>().Result;
                return View();
                // TODO: Add insert logic here

            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/CodeDiscount/Edit/5
        [AuthorizeLoginAdmin]
        public ActionResult Edit(string id)
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/CodeDiscount/getbyid/"+id);
            responseMessage.EnsureSuccessStatusCode();
            DTO_CodeDiscount dTO_Accounts = responseMessage.Content.ReadAsAsync<DTO_CodeDiscount>().Result;
            return View(dTO_Accounts);
        }

        // POST: Admin/CodeDiscount/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection, DTO_CodeDiscount request)
        {
            try
            {
                // TODO: Add update logic here

                var discount = double.Parse(collection["SelectDiscount"]);
                request.Discount = discount/100;
                request.Content = collection["SelectDiscount"] + "%";
                HttpResponseMessage responseMessage = service.PostResponse("api/CodeDiscount/update/", request);
                responseMessage.EnsureSuccessStatusCode();
                bool result = responseMessage.Content.ReadAsAsync<bool>().Result;
                return View();
            }
            catch
            {
                return View();
            }
        }

        [AuthorizeLoginAdmin]
        public ActionResult Delete(string id)
        {
            try
            {
                HttpResponseMessage response = service.DeleteResponse("api/CodeDiscount/Deletecodediscount/" + id);
                response.EnsureSuccessStatusCode();
                return Json(new { mes = true });
            }
            catch
            {
                return Json(new { mes = false });
            }
        }
    }
}
