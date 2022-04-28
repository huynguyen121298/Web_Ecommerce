using Model.DTO.DTO_Ad;
using Model.DTO_Model;
using PagedList;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using UI.Service;

namespace UI.Controllers
{
    public class MerchantController : Controller
    {
        ServiceRepository service = new ServiceRepository();
        // GET: Merchant
        public ActionResult Index(FormCollection fc, int? page)
        {
            //get session
            if (page == null) page = 1;
            int pageSize = 25;

            int pageNumber = (page ?? 1);
            var searchMerchant = fc["searchMerchant"];

            //Get product by merchant
            if (searchMerchant != null && searchMerchant != "")
            {
                HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetMerchantByName/" + searchMerchant);
                responseMessage2.EnsureSuccessStatusCode();

                List<DTO_Account2> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Account2>>().Result;
                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            return View();

        }

        public ActionResult ProductByMerchant(string merchantId)
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Product/GetProductByMerchant/"+merchantId);
            responseMessage.EnsureSuccessStatusCode();
            List<List<DTO_Dis_Product>> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<List<DTO_Dis_Product>>>().Result;
            var view = dTO_Accounts.ToPagedList(1, 50);

            return View(view);
        }
    }
}