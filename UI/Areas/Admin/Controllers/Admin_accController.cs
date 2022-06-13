using Model.DTO.DTO_Ad;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UI.Areas.Admin.Common;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class Admin_accController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        [AuthorizeLoginAdmin]
        [DeatAuthorize(Order = 2)]
        public ActionResult Index()
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Admin_acc/getallaccounts");
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Account> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Account>>().Result;
            return View(dTO_Accounts);
        }

        [AuthorizeLoginAdmin]
        public ActionResult Edit(string id)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseMessage = service.GetResponse("api/Admin_acc/GetAccountById/" + id);
            responseMessage.EnsureSuccessStatusCode();
            DTO_Account2 dtoAccounts = responseMessage.Content.ReadAsAsync<DTO_Account2>().Result;

            return View(dtoAccounts);
        }

        [AuthorizeLoginAdmin]
        public ActionResult Details(string id)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseMessage = service.GetResponse("api/Admin_acc/GetAccountById/" + id);
            responseMessage.EnsureSuccessStatusCode();
            DTO_Account dtoAccounts = responseMessage.Content.ReadAsAsync<DTO_Account>().Result;

            return View(dtoAccounts);
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/images_merchant/" + file.FileName));

            return "~/images_merchant/" + file.FileName;
        }

        [AuthorizeLoginAdmin]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(DTO_Account2 dTO_Account, HttpPostedFileBase ImageUpload)
        {
            var stt = Request.Form["stt"];
            var pass = Request.Form["pass"];
            if (stt == "Admin")
            {
                dTO_Account.RoleId = 1;
                if (pass != "")
                {
                    dTO_Account.Password = pass;
                    HttpResponseMessage response = service.PostResponse("api/Admin_acc/UpdateAcc/", dTO_Account);
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    HttpResponseMessage response = service.PostResponse("api/Admin_acc/UpdateAccTwo/", dTO_Account);
                    response.EnsureSuccessStatusCode();
                }
                return RedirectToAction("Index");

            }
            else
            {
                if (ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                    string extension = Path.GetExtension(ImageUpload.FileName);
                    fileName = fileName + extension;
                    dTO_Account.Photo = "~/images_merchant/" + fileName;
                    ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/images_merchant/"), fileName));
                    var adminLogin = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];



                    if (dTO_Account._id == adminLogin._id)
                    {
                        var userSession = new DTO_Account();
                        userSession = adminLogin;
                        userSession.Photo = dTO_Account.Photo;
                        Session.Remove(CommonConstants.ACCOUNT_SESSION);
                        Session.Add(CommonConstants.ACCOUNT_SESSION, userSession);
                    }
                }

                dTO_Account.RoleId = 2;
                if (pass != "")
                {
                    dTO_Account.Password = pass;
                    HttpResponseMessage response = service.PostResponse("api/Admin_acc/UpdateAcc/", dTO_Account);
                    response.EnsureSuccessStatusCode();
                }
                else
                {

                    HttpResponseMessage response = service.PostResponse("api/Admin_acc/UpdateAccTwo/", dTO_Account);
                    response.EnsureSuccessStatusCode();
                }
                Response.Cookies["firstname1"].Value = dTO_Account.FirstName + "" + dTO_Account.LastName;

                //HttpCookie cknameAccount1 = new HttpCookie("firstname1");
                //cknameAccount1.Value = dTO_Account.FirstName + dTO_Account.LastName;
                //Response.Cookies.Add(cknameAccount1);
                //HttpCookie ck1 = new HttpCookie("firstname1", (resultLogin.FirstName + "  " + resultLogin.LastName).ToString());
                //ck1.Expires = DateTime.Now.AddHours(48);
                //Response.Cookies.Add(ck1);

                var userLogin = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
                if (userLogin.RoleId == 2)
                {
                    return RedirectToAction("Index", "Admin");

                }
                return RedirectToAction("Index");


            }

        }

        [AuthorizeLoginAdmin]
        public ActionResult Create()
        {


            return View();

        }

        [AuthorizeLoginAdmin]
        public ActionResult Delete(string id)
        {
            try
            {
                HttpResponseMessage response = service.DeleteResponse("api/Admin_Acc/DeleteAccount/" + id);
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