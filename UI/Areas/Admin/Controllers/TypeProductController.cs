using Model.DTO.DTO_Ad;
using Model.DTO.DTO_Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class TypeProductController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        // GET: Admin/CodeDiscount
        [DeatAuthorize(Order = 2)]
        [AuthorizeLoginAdmin]
        public ActionResult Index()
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/ItemType/getall");
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Item_Type> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Item_Type>>().Result;
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
        public ActionResult Create(DTO_Item_Type request)
        {
            try
            {
               
                HttpResponseMessage responseMessage = service.PostResponse("api/ItemType/create/",request);
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
            HttpResponseMessage responseMessage = service.GetResponse("api/ItemType/getbyid/"+id);
            responseMessage.EnsureSuccessStatusCode();
            DTO_Item_Type dTO_Accounts = responseMessage.Content.ReadAsAsync<DTO_Item_Type>().Result;
            return View(dTO_Accounts);
        }

        // POST: Admin/CodeDiscount/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection, DTO_Item_Type request)
        {
            try
            {
                // TODO: Add update logic here

                request.Status = collection["sttItemType"];
                HttpResponseMessage responseMessage = service.PostResponse("api/ItemType/update/", request);
                responseMessage.EnsureSuccessStatusCode();
                bool result = responseMessage.Content.ReadAsAsync<bool>().Result;
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
