using Model.DTO.DTO_Ad;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class Users_AccController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        [AuthorizeLoginAdmin]
        // GET: Admin/Admin_acc
        [DeatAuthorize(Order = 2)]
        public ActionResult Index()
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/User_acc/getallaccounts");
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_User_Acc> DTO_User_Accs = responseMessage.Content.ReadAsAsync<List<DTO_User_Acc>>().Result;
            return View(DTO_User_Accs);
        }

        [AuthorizeLoginAdmin]
        public ActionResult Edit(string id)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseMessage = service.GetResponse("api/User_acc/GetAccountById/" + id);
            responseMessage.EnsureSuccessStatusCode();
            DTO_User_Acc dtoAccounts = responseMessage.Content.ReadAsAsync<DTO_User_Acc>().Result;

            return View(dtoAccounts);
        }

        [AuthorizeLoginAdmin]
        public ActionResult Details(string id)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseMessage = service.GetResponse("api/User_acc/GetAccountById/" + id);
            responseMessage.EnsureSuccessStatusCode();
            DTO_User_Acc dtoAccounts = responseMessage.Content.ReadAsAsync<DTO_User_Acc>().Result;

            return View(dtoAccounts);
        }

        [AuthorizeLoginAdmin]
        [HttpPost]
        public ActionResult Edit(DTO_User_Acc DTO_User_Acc)
        {
            var pass = Request.Form["pass"];
            if (pass != "")
            {
                DTO_User_Acc.Password = pass;
                HttpResponseMessage response = service.PostResponse("api/User_acc/UpdateUser/", DTO_User_Acc);
                response.EnsureSuccessStatusCode();
            }
            else
            {
                HttpResponseMessage response = service.PostResponse("api/User_acc/UpdateUserTwo/", DTO_User_Acc);
                response.EnsureSuccessStatusCode();
            }
            return RedirectToAction("Index");
        }

        [AuthorizeLoginAdmin]
        public ActionResult Delete(string id)
        {
            try
            {
                HttpResponseMessage response = service.DeleteResponse("api/User_Acc/DeleteAccount/" + id);
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
