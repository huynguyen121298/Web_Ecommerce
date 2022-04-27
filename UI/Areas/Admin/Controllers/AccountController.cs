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
            DTO_Account model = CheckAccount();
            if (model == null)
                return View();
            else
            {
                //Save token API
                Session.Add(CommonConstants.ACCOUNT_SESSION, model);
                //string tokenUser = GetToken(model.Email);
                HttpCookie cookie = new HttpCookie("firstname1", model.FirstName);
                cookie.Expires = DateTime.Now.AddHours(48);
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Admin");
            }
        }
        public DTO_Account CheckAccount()
        {
            DTO_Account result = null;

            string firstname = string.Empty;
            string accountId = string.Empty;

            if (Request.Cookies["firstname1"] != null)
                firstname = Request.Cookies["firstname1"].Value;

            if (Request.Cookies["accountId"] != null)
                accountId = Request.Cookies["accountId"].Value;


            if (!string.IsNullOrEmpty(firstname))
                result = new DTO_Account { FirstName = firstname, _id = accountId };
            return result;
        }
        [DeatAuthorize(Order = 2)]
        public ActionResult AddRegister()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [DeatAuthorize(Order = 2)]
        public ActionResult AddRegister(RegisterAdminModel model)
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
                    RoleId = model.RoleId,
                    MerchantName = model.MerchantName
                };
                var stt = Request.Form["stt"];
                var merchantInput = Request.Form["merchantInput"];
                if (stt == "Admin")
                {
                    c.MerchantName = merchantInput;
                    c.RoleId = 1;

                    HttpResponseMessage response = serviceObj.PostResponse(url + "InsertAccount/", c);
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    if(merchantInput == "")
                    {
                        
                        ViewData["ErrorMessage"] = "Tên doanh nghiệp không được để trống";
                        return View();
                    }
                    c.MerchantName = merchantInput;
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

                HttpCookie ck2 = new HttpCookie("accountId", resultLogin._id);
                ck2.Expires = DateTime.Now.AddHours(48);
                Response.Cookies.Add(ck2);

                var userSession = new DTO_Account();
                userSession.Email = resultLogin.Email;
                userSession.RoleId = resultLogin.RoleId;
                userSession._id = resultLogin._id;
                userSession.Photo = resultLogin.Photo;
                Session.Add(CommonConstants.ACCOUNT_SESSION, userSession);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewData["ErrorMessage"] = ("Tên đăng nhập hoặc mật khẩu không tồn tại.");
                return this.View();
            }
            

        }
        public ActionResult Logout()
        {
            try
            {
                if (Session[CommonConstants.ACCOUNT_SESSION] != null)
                {
                    Session.Remove(CommonConstants.ACCOUNT_SESSION);
                }
                


                if (Request.Cookies["firstname1"] != null)
                {
                    HttpCookie cknameAccount1 = new HttpCookie("firstname1");
                    cknameAccount1.Expires = DateTime.Now.AddHours(-50);
                    Response.Cookies.Add(cknameAccount1);
                }

                if (Request.Cookies["accountId"] != null)
                {
                    HttpCookie ckAccount = new HttpCookie("accountId");
                    ckAccount.Expires = DateTime.Now.AddHours(-50);
                    Response.Cookies.Add(ckAccount);
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
