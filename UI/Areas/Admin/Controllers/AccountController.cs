using Model.DTO.DTO_Ad;
using System;
using System.Globalization;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UI.Areas.Admin.Common;
using UI.Areas.Admin.Models;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private string url;
        private ServiceRepository serviceObj;
      
        public AccountController()
        {
            serviceObj = new ServiceRepository();
            url = "api/Account/";

        }

        [HttpGet]
        public ActionResult Login()
        {
            AdminLogin model = CheckAccount();
            if (model == null)
                return View();
            else
            {
                //Save token API
                Session[CommonConstants.ACCOUNT_SESSION] = model;
                //string tokenUser = GetToken(model.Email);
                HttpCookie cookie = new HttpCookie("firstname1");
                cookie.Expires = DateTime.Now.AddHours(48);
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Admin", new { usr = model.Email });
            }
        }
        public AdminLogin CheckAccount()
        {
            AdminLogin result = null;

            string firstname = string.Empty;

            if (Request.Cookies["firstname1"] != null)
                firstname = Request.Cookies["firstname1"].Value;


            if (!string.IsNullOrEmpty(firstname))
                result = new AdminLogin { FirstName = firstname };
            return result;
        }
        public ActionResult AddRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [DeatAuthorize(Order = 2)]
        public ActionResult AddRegister([Bind(Include = "FirstName,LastName,Email,Password,ConfirmPassword")] RegisterAdminModel model, string AuthenticationCode)
        {

            if (ModelState.IsValid)
            {

                //check email đã đc dùng
                var mail = GetAccountByEmail(model.Email);
                if (mail != null)
                {
                    ModelState.AddModelError("", "Email này đã được sử dụng.");
                    return View(model);
                }

                DTO_Account c = new DTO_Account
                {
                    FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.FirstName.Trim().ToLower()),
                    LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.LastName.Trim().ToLower()),
                    Password = model.Password,
                    Email = model.Email,
                    RoleId = model.RoleId
                };
                var stt = Request.Form["stt"];
                if (stt == "Admin")
                {
                    c.RoleId = 1;

                    HttpResponseMessage response = serviceObj.PostResponse(url + "InsertAccount/", c);
                    response.EnsureSuccessStatusCode();
                }
                else
                {

                    c.RoleId = 2;

                    HttpResponseMessage response = serviceObj.PostResponse(url + "InsertAccount/", c);
                    response.EnsureSuccessStatusCode();
                }
                ModelState.Clear();
                ViewData["ErrorMessage"] = "Đăng ký thành công";
                return View();
            }
            else
            {
                ViewData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin";
                return View(model);
            }
        }

        public DTO_Account GetAccountByEmail(string mail)
        {
            HttpResponseMessage response = serviceObj.GetResponse(url + "GetAccountByEmail?email=" + mail);
            response.EnsureSuccessStatusCode();
            DTO_Account result = response.Content.ReadAsAsync<DTO_Account>().Result;
            return result;
        }
        public ActionResult NotAuthorize()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(AdminLogin model)
        {

            HttpResponseMessage response = serviceObj.GetResponse(url + "GetLoginResultByEmailPassword?user=" + model.Email + "&pass=" + model.Password);
            response.EnsureSuccessStatusCode();
            DTO_Account resultLogin = response.Content.ReadAsAsync<DTO_Account>().Result;
            //
            bool acc = (resultLogin.Email != null);
            if (acc)
            {
                HttpCookie ck1 = new HttpCookie("firstname1", (resultLogin.FirstName + "  " + resultLogin.LastName).ToString());
                ck1.Expires = DateTime.Now.AddHours(48);
                Response.Cookies.Add(ck1);

                var userSession = new DTO_Account();
                userSession.Email = resultLogin.Email;
                userSession.RoleId = resultLogin.RoleId;
                userSession.AccountId = resultLogin.AccountId;  
                Session.Add(CommonConstants.ACCOUNT_SESSION, userSession);
                return View("~/Areas/Admin/Views/Admin/Index.cshtml");
            }
            else
            {
                ViewData["ErrorMessage"] = ("Tên đăng nhập hoặc mật khẩu không tồn tại.");
            }
            return this.View();

        }
        public ActionResult Logout()
        {
            try
            {
                Session.Remove(CommonConstants.ACCOUNT_SESSION);


                if (Request.Cookies["firstname1"] != null)
                {
                    HttpCookie cknameAccount1 = new HttpCookie("firstname1");
                    cknameAccount1.Expires = DateTime.Now.AddHours(-50);
                    Response.Cookies.Add(cknameAccount1);
                }
                //Session.Clear();
                Session.Abandon();
                //Response.Cookies.Clear();
                //Request.Cookies.Clear(); 
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                return View("Index");
            }
        }
    }
}
