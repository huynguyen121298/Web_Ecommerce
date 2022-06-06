using Model.DTO.DTO_Ad;
using Model.DTO.DTO_Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class ItemController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        public ActionResult Index(string productId)
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/ItemType/getitem/"+productId);
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

        public ActionResult Update(DTO_Item request)
        {
            try
            {
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
    }
}