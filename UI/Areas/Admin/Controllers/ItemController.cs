using Model.DTO.DTO_Ad;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class ItemController : Controller
    {
        private ServiceRepository service = new ServiceRepository();

        public ActionResult Index(string productId)
        {
            if (productId != null)
                Session.Add("productId", productId);
            productId = (string)Session["productId"];
            ViewBag.productId = productId ;
            HttpResponseMessage responseMessage = service.GetResponse("api/ItemType/getitem/" + productId);
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Item> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Item>>().Result;
            
          
            return View(dTO_Accounts);
        }

        public ActionResult Details(string itemId)
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/ItemType/ItemDetails/" + itemId);
            responseMessage.EnsureSuccessStatusCode();
            DTO_Item dTO_Accounts = responseMessage.Content.ReadAsAsync<DTO_Item>().Result;
            return View(dTO_Accounts);
        }

        public ActionResult Update(DTO_Item request, string selectColor, string sizeProduct)
        {
            try
            {
                request.Color = selectColor ?? null;
                if (sizeProduct != string.Empty)
                    request.Size = sizeProduct;
                HttpResponseMessage responseMessage = service.PostResponse("api/ItemType/updateItem/", request);
                responseMessage.EnsureSuccessStatusCode();
                bool result = responseMessage.Content.ReadAsAsync<bool>().Result;
                if (result)
                    return View("Index");
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        [AuthorizeLoginAdmin]
        public ActionResult Create()
        {
           

            return View();
        }

        // POST: Admin/CodeDiscount/Create
        [HttpPost]
        public ActionResult Create(DTO_Item request, string selectColor, string sizeProduct)
        {
            try
            {

                request.Color = selectColor?? null;
                if(sizeProduct != string.Empty)
                    request.Size = sizeProduct;
                request.ProductId = (string)Session["productId"];
                HttpResponseMessage responseMessage = service.PostResponse("api/ItemType/createItem/", request);
                responseMessage.EnsureSuccessStatusCode();
                bool result = responseMessage.Content.ReadAsAsync<bool>().Result;
                if (result)
                    return RedirectToAction("Index","Item");
                else
                    return View();
                // TODO: Add insert logic here
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
                HttpResponseMessage response = service.DeleteResponse("api/ItemType/DeleteItem/" + id);
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