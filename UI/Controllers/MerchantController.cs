using Model.Common;
using Model.DTO.DTO_Ad;
using Model.DTO_Model;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using UI.Service;

namespace UI.Controllers
{
    public class MerchantController : Controller
    {
        private ServiceRepository service = new ServiceRepository();

        // GET: Merchant
        public ActionResult Index(FormCollection fc, int? page)
        {
            //get session
            if (page == null) page = 1;
            int pageSize = 25;

            int pageNumber = (page ?? 1);
            var searchMerchant = (string)Session[Constants.SEARCHMERCHANT];

            //Get product by merchant
            if (searchMerchant != null && searchMerchant != "")
            {
                HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetMerchantByName/" + searchMerchant);
                responseMessage2.EnsureSuccessStatusCode();

                List<DTO_Account2> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Account2>>().Result;
                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            var merchant = new List<DTO_Account2>();
            return View(merchant.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index2(string id)
        {
            string merchantId = (string)Session[Constants.MERCHANT_ID];

            HttpResponseMessage responseMessage = service.GetResponse("api/Merchant/GetAllProductByIdItemClient/" + id + "/" + merchantId);
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
            if (dTO_Accounts == null)
            {
                return Content("Chưa có sản phẩm bạn đang muốn tìm kiếm");
            }
            ViewData["TypeProduct"] = dTO_Accounts.Select(s=>s.Type_Product).FirstOrDefault();
            var view = dTO_Accounts.ToPagedList(1, 10);
            return View(view);
        }

        public ActionResult ProductByMerchant(string merchantId, FormCollection fc, int? page, int? gia, int? gia_, string searchName1)
        {
            var searchName = fc["searchNameMer"];
            var priceGiaMin = Request.Form["priceGiaMinMer"];
            var priceGiaMax = Request.Form["priceGiaMaxMer"];

            if (merchantId != null)
            {
                Session.Add(Constants.MERCHANT_ID, merchantId);
            }
            else
            {
                merchantId = (string)Session[Constants.MERCHANT_ID];
            }

            if (page == null) page = 1;
            int pageSize = 25;

            int pageNumber = (page ?? 1);

            if ((searchName != null && searchName != "") || (searchName1 != "" && searchName1 != null))
            {
                try
                {
                    HttpResponseMessage responseMessage2 = service.GetResponse("api/Merchant/GetAllProductByName/" + searchName + "/" + merchantId);
                    responseMessage2.EnsureSuccessStatusCode();

                    List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                    return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
                }
                catch
                {
                    HttpResponseMessage responseMessage3 = service.GetResponse("api/Merchant/GetAllProductByName/" + searchName1 + "/" + merchantId);
                    responseMessage3.EnsureSuccessStatusCode();

                    List<DTO_Dis_Product> dTO_Accounts3 = responseMessage3.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                    return View(dTO_Accounts3.ToPagedList(pageNumber, pageSize));
                }
            }

            if (priceGiaMin != null && priceGiaMax != null && priceGiaMin != "" && priceGiaMax != "")
            {
                HttpResponseMessage responseMessage2 = service.GetResponse("api/Merchant/GetAllProductByPrice/" + priceGiaMin + "/" + priceGiaMax + "/" + merchantId);
                responseMessage2.EnsureSuccessStatusCode();

                List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            try
            {
                HttpResponseMessage responseMessage2 = service.GetResponse("api/Merchant/GetAllProductByPrice/" + gia_ + "/" + gia + "/" + merchantId);
                responseMessage2.EnsureSuccessStatusCode();

                List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                HttpResponseMessage responseMessage = service.GetResponse("api/Product/GetProductByMerchant/" + merchantId);
                responseMessage.EnsureSuccessStatusCode();
                List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                var view = dTO_Accounts.ToPagedList(1, 50);

                HttpResponseMessage responseMessage2 = service.GetResponse("api/Admin_acc/GetAccountById/" + merchantId);
                responseMessage2.EnsureSuccessStatusCode();

                DTO_Account2 dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<DTO_Account2>().Result;

                ViewData["MerchantName"] = dTO_Accounts2.MerchantName;
                Session.Add(Constants.MERCHANT_NAME, dTO_Accounts2.MerchantName); 
                

                return View(view);
            }


        }

        public ActionResult Index_(DTO_Dis_Product dTO_Product, int? page)
        {
            string merchantId = (string)Session[Constants.MERCHANT_ID];
            if (page == null) page = 1;
            int pageSize = 25;
            int pageNumber = (page ?? 1);

            HttpResponseMessage responseMessage = service.GetResponse("api/Merchant/GetAllProduct_DiscountByEndUser/"+merchantId);
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;

            return View(dTO_Accounts.ToPagedList(pageNumber, pageSize));
        }
    }
}