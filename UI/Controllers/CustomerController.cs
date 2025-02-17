﻿using Facebook;
using Model.Common;
using Model.DTO.DTO_Client;
using Model.DTO_Model;
using MongoDB.Bson;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UI.Models;
using UI.Security_;
using UI.Service;

namespace UI.Controllers
{
    public class CustomerController : Controller
    {
        private string url;
        private ServiceRepository serviceObj;

        public ActionResult Index()
        {
            return View();
        }

        public CustomerController()
        {
            serviceObj = new ServiceRepository();
            url = "api/Customer/";
        }

        public ActionResult Logout()
        {
            try
            {
                Session.Remove(Constants.USER_SESSION);
                Session.Clear();

                if (Request.Cookies["usernameCustomer"] != null)
                {
                    HttpCookie ckUserAccount = new HttpCookie("usernameCustomer");
                    ckUserAccount.Expires = DateTime.Now.AddHours(-48);
                    Response.Cookies.Add(ckUserAccount);
                }
                if (Request.Cookies["idCustomer"] != null)
                {
                    HttpCookie ckIDAccount = new HttpCookie("idCustomer");
                    ckIDAccount.Expires = DateTime.Now.AddHours(-48);
                    Response.Cookies.Add(ckIDAccount);
                }
                if (Request.Cookies["nameCustomer"] != null)
                {
                    HttpCookie cknameAccount = new HttpCookie("nameCustomer");
                    cknameAccount.Expires = DateTime.Now.AddHours(-48);
                    Response.Cookies.Add(cknameAccount);
                }

                if (Request.Cookies["nameCustomer1"] != null)
                {
                    HttpCookie cknameAccount1 = new HttpCookie("nameCustomer1");
                    cknameAccount1.Expires = DateTime.Now.AddHours(-48);
                    Response.Cookies.Add(cknameAccount1);
                }

                if (Request.Cookies[Constants.TOKEN_NUMBER] != null)
                {
                    HttpCookie cookie = new HttpCookie(Constants.TOKEN_NUMBER);
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
                if (Request.Cookies["firstname"] != null)
                {
                    HttpCookie ck1 = new HttpCookie("firstname");
                    ck1.Expires = DateTime.Now.AddHours(-48);
                    Response.Cookies.Add(ck1);
                }
                Session.Remove(Constants.ADMIN_SESSION);
                if (Response.Cookies["email"] != null)
                {
                    HttpCookie ckUser = new HttpCookie("email");
                    ckUser.Expires = DateTime.Now.AddHours(-48);
                    Response.Cookies.Add(ckUser);
                }
                if (Response.Cookies["password"] != null)
                {
                    HttpCookie ckPass = new HttpCookie("password");
                    ckPass.Expires = DateTime.Now.AddHours(-48);
                    Response.Cookies.Add(ckPass);
                }
                Session.Abandon();

                Constants.COUNT_LOGIN_FAIL_USER = 0;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return View("Login");
            }
        }

        public ActionResult Login()
        {
            UserLogin model = CheckAccount();
            if (model == null)
                return View();
            else
            {
                HttpCookie ck1 = new HttpCookie("firstname", (model.FirstName + "  " + model.LastName).ToString());
                ck1.Expires = DateTime.Now.AddHours(48);
                Response.Cookies.Add(ck1);
                //Save token API
                Session[Constants.USER_SESSION] = model;
                Session.Timeout = 100;

                return RedirectToAction("ProfileUser", "Customer");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin model)
        {
            HttpResponseMessage response = serviceObj.GetResponse(url + "GetLoginResultByUsernamePassword?user=" + model.Email + "&pass=" + model.Password);
            response.EnsureSuccessStatusCode();
            DTO_Users_Acc resultLogin = response.Content.ReadAsAsync<DTO_Users_Acc>().Result;
            //
            bool acc = (resultLogin.Email != null);
            if (acc)
            {
                //HttpResponseMessage responseUser = serviceObj.GetResponse(url + "GetCustomerByUsername?user=" + model.Email);
                //responseUser.EnsureSuccessStatusCode();
                //DTO_Users_Acc customLogin = responseUser.Content.ReadAsAsync<DTO_Users_Acc>().Result;

                if (model.RememberMe)
                {
                    

               
                    //Save cookies
                    HttpCookie ckUserAccount = new HttpCookie("usernameCustomer");
                    ckUserAccount.Expires = DateTime.Now.AddHours(48);
                    ckUserAccount.Value = resultLogin.Email;
                    Response.Cookies.Add(ckUserAccount);
                    HttpCookie ckIDAccount = new HttpCookie("idCustomer");
                    ckIDAccount.Expires = DateTime.Now.AddHours(48);
                    ckIDAccount.Value = resultLogin._id + "";
                    Response.Cookies.Add(ckIDAccount);

                    HttpCookie ckNameAccount = new HttpCookie("nameCustomer");
                    ckNameAccount.Expires = DateTime.Now.AddHours(48);
                    ckNameAccount.Value = resultLogin.FirstName + "  " + resultLogin.LastName;
                    Response.Cookies.Add(ckNameAccount);
                }
                UserLogin u = new UserLogin
                {
                    _id = resultLogin._id,
                    FirstName = resultLogin.FirstName,
                    LastName = resultLogin.LastName,
                    Email = resultLogin.Email,
                    PhoneNumber = resultLogin.PhoneNumber,
                    Address = resultLogin.Address,
                    City = resultLogin.City,
                };
                Session.Add(Constants.USER_SESSION, u);
                Session.Timeout = 1000;
                HttpCookie ck1 = new HttpCookie("firstname", (resultLogin.FirstName + "  " + resultLogin.LastName).ToString());
                ck1.Expires = DateTime.Now.AddHours(48);
                Response.Cookies.Add(ck1);

                ViewBag.SuccessMessage = "Đăng nhập thành công";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            return this.View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup([Bind(Include = "FirstName,LastName,Email,Password,ConfirmPassword")] RegisterModel model, string AuthenticationCode)
        {
            var id = Request.Form["AuthenticationCode"];
            if (id == "")
            {
                ViewData["ErrorMessage5"] = "Mã xác thực không được để trống";
            }
            var authenticationEmail = (AuthenticationEmail)Session[Constants.AUTHENTICATIONEMAIL_SESSION];
            if (ModelState.IsValid & authenticationEmail != null)
            {
                if (model.Email == authenticationEmail.Email & authenticationEmail.AuthenticationCode == AuthenticationCode)               
                {
                    //check email đã đc dùng
                    var mail = GetCustomerByEmail(model.Email);
                    if (mail != null)
                    {
                        ModelState.AddModelError("", "Email này đã được sử dụng.");
                        return View(model);
                    }

                    DTO_Users_Acc c = new DTO_Users_Acc
                    {
                        FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.FirstName.Trim().ToLower()),
                        LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.LastName.Trim().ToLower()),
                        Password = model.Password,
                        Email = model.Email,
                    };

                    HttpResponseMessage response = serviceObj.PostResponse(url + "InsertCustomer/", c);
                    response.EnsureSuccessStatusCode();

                    Session[Constants.USER_SESSION] = null;
                    return RedirectToAction("Login", "Customer");
                }
                else
                {
                    ViewData["ErrorMessage"] = "Mã xác thực không hợp lệ";
                    return View(model);
                }
            }
            return View(model);
        }

        public DTO_Users_Acc GetCustomerByEmail(string mail)
        {
            HttpResponseMessage response = serviceObj.GetResponse(url + "GetCustomerByEmail?mail=" + mail);
            response.EnsureSuccessStatusCode();
            DTO_Users_Acc result = response.Content.ReadAsAsync<DTO_Users_Acc>().Result;
            return result;
        }

        public string GetCustomerByPassword(string email)
        {
            HttpResponseMessage response = serviceObj.GetResponse(url + "GetCustomerByPassword?email=" + email);
            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public string GetToken(string username)
        {
            HttpResponseMessage response = serviceObj.GetResponse(url + "GetToken?username=" + username);
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsAsync<string>().Result;
        }

        public UserLogin CheckAccount()
        {
            UserLogin result = null;
            string username = string.Empty;
            string id = string.Empty;
            string firstname = string.Empty;
            string lastname = string.Empty;
            string email = string.Empty;
            string password = string.Empty;

            if (Request.Cookies["usernameCustomer"] != null)
                username = Request.Cookies["usernameCustomer"].Value;
            if (Request.Cookies["idCustomer"] != null)
                id = Request.Cookies["idCustomer"].Value;
            if (Request.Cookies["nameCustomer"] != null)
                firstname = Request.Cookies["nameCustomer"].Value;

            if (!string.IsNullOrEmpty(username) & !string.IsNullOrEmpty(password))
                if (!string.IsNullOrEmpty(firstname) & !string.IsNullOrEmpty(id) & !string.IsNullOrEmpty(firstname))
                    result = new UserLogin { _id = id, Password = password, Email = email, FirstName = firstname, LastName = lastname };
            return result;
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                string resetCode = Guid.NewGuid().ToString();
                var verifyUrl = "/Customer/ResetPassword?id=" + resetCode + "&mail=" + model.Email;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
                HttpResponseMessage response = serviceObj.GetResponse(url + "GetCustomerByEmail?mail=" + model.Email);
                response.EnsureSuccessStatusCode();
                DTO_Users_Acc result = response.Content.ReadAsAsync<DTO_Users_Acc>().Result;

                if (result != null)
                {
                    string FullName = result.FirstName + result.LastName;
                    ResetPasswordModel resetModel = new ResetPasswordModel();
                    resetModel.ResetCode = resetCode;
                    resetModel._id = result._id;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property
                    var subject = "Password Reset Request";
                    var body = "Hi " + FullName + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +
                            " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                            "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";
                    SendEmail(result.Email, body, subject);
                    ModelState.AddModelError("", "Mã code đã được gửi vào email của bạn");
                }
                else
                    ModelState.AddModelError("", "Địa chỉ email không tồn tại.");
                return View();
            }
            return View();
        }

        private void SendEmail(string emailAddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("huytuannguyen.301198@gmail.com", emailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("huytuannguyen.301198@gmail.com", "Huyhuy123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

        public ActionResult ResetPassword2(string id, string mail)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword2(ResetPasswordModel2 model, string id, string mail)
        {
            DTO_Users_Acc resultReset = GetCustomerByEmail(mail);
            string resultPass = GetCustomerByPassword(model.oldPassword);
            if (resultPass != "")
            {
                resultReset.Password = model.NewPassword;
                resultReset.Email = model.Mail;
                resultReset._id = id;

                HttpResponseMessage responseUpdate = serviceObj.PutResponse(url + "UpdateCustomer3", resultReset);
                responseUpdate.EnsureSuccessStatusCode();
                bool result = responseUpdate.Content.ReadAsAsync<bool>().Result;
                if (result == true)
                    return RedirectToAction("ProfileUser");
                else
                {
                    ViewBag.Warning = "Có lỗi xảy ra trong quá trình đặt lại mật khẩu.";
                    return this.View();
                }
            }
            else
            {
                ViewBag.Warning = "Mật khẩu cũ nhập không đúng";
                return this.View();
            }
        }

        public ActionResult ResetPassword(string id, string mail)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrEmpty(mail))
            {
                return View("~/Views/Shared/Error_.cshtml");
            }
            DTO_Users_Acc result = GetCustomerByEmail(mail);
            ResetPasswordModel mode = new ResetPasswordModel();
            mode._id = result._id;
            mode.Mail = mail;
            mode.ResetCode = id;
            return View(mode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            DTO_Users_Acc resultReset = GetCustomerByEmail(model.Mail);
            resultReset.Password = model.NewPassword;
            HttpResponseMessage responseUpdate = serviceObj.PutResponse(url + "UpdateCustomer2", resultReset);
            responseUpdate.EnsureSuccessStatusCode();
            bool result = responseUpdate.Content.ReadAsAsync<bool>().Result;
            if (result)
                return RedirectToAction("Login");
            ViewBag.Warning = "Có lỗi xảy ra trong quá trình đặt lại mật khẩu.";
            return this.View();
        }

        public DTO_Users_Acc GetCustomerByUsername()
        {
            UserLogin sess = (UserLogin)Session[Constants.USER_SESSION];
            HttpResponseMessage response = serviceObj.GetResponse(url + "GetCustomerByUsername?user=" + sess.Email);
            response.EnsureSuccessStatusCode();
            DTO_Users_Acc result = response.Content.ReadAsAsync<DTO_Users_Acc>().Result;
            return result;
        }

        public DTO_Users_Acc GetCustomerByToken(string token)
        {
            HttpResponseMessage response = serviceObj.GetResponse(url + "GetCustomerByToken?token=" + token);
            response.EnsureSuccessStatusCode();
            DTO_Users_Acc result = response.Content.ReadAsAsync<DTO_Users_Acc>().Result;
            return result;
        }

        public JsonResult GetAuthenticationInEmail(string Email)
        {
            var mail = GetCustomerByEmail(Email);
            if (mail != null)
            {
                return Json(new { status = false });
            }
            Session[Constants.AUTHENTICATIONEMAIL_SESSION] = null;
            //int authCode = new Random().Next(1000, 9999);
            int authCode = 123456;

            AuthenticationEmail authenticationEmail = new AuthenticationEmail();
            authenticationEmail.Email = Email;
            authenticationEmail.AuthenticationCode = authCode.ToString();
            Session[Constants.AUTHENTICATIONEMAIL_SESSION] = authenticationEmail;

            //MailHelper.SendMailAuthentication(Email, "Mã xác thực từ H_Shop", authCode.ToString());

            return Json(new { status = true });
            
        }

        [AuthorizeLoginEndUser]
        public PartialViewResult ProductBought(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 25;
            int pageNumber = (page ?? 1);

            var userLogin = (UserLogin)Session[Constants.USER_SESSION];
            HttpResponseMessage responseUser = serviceObj.GetResponse("api/product/GetProductsBought/" + userLogin._id);

            responseUser.EnsureSuccessStatusCode();
            List<DTO_Dis_Product> result = responseUser.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;

            return PartialView(result.ToPagedList(pageNumber,pageSize));
        }

        [AuthorizeLoginEndUser]
        public ActionResult ProductFavorite()
        {
            var userLogin = (UserLogin)Session[Constants.USER_SESSION];
            HttpResponseMessage responseUser = serviceObj.GetResponse("api/product/GetProductsFavorite/" + userLogin._id);

            responseUser.EnsureSuccessStatusCode();
            List<DTO_Product_Item_Type> result = responseUser.Content.ReadAsAsync<List<DTO_Product_Item_Type>>().Result;
            if (result == null)
            {
                return RedirectToAction("Thankyou1","Cart");
            }


            return PartialView(result);
        }

        public ActionResult UserPartial()
        {
            return PartialView();
        }

        [AuthorizeLoginEndUser]
        [HttpGet]
        public ActionResult ProfileUser()

        {
            DTO_Users_Acc cus = GetCustomerByUsername();
            return View(cus);
        }

        [AuthorizeLoginEndUser]
        [HttpPost]
        public ActionResult ProfileUser(DTO_Users_Acc model)
        {
            HttpResponseMessage response = serviceObj.PutResponse(url + "UpdateCustomer", model);
            bool resultUpdate = response.Content.ReadAsAsync<bool>().Result;

            var newUserLogin = new UserLogin()
            {
                _id = model._id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                City = Request.Form["city"].ToString(),
            };
            
           
            Session.Add(Constants.USER_SESSION,newUserLogin);

            if (Request.Cookies["firstname"] != null)
            {
                HttpCookie ck = new HttpCookie("firstname");
                ck.Expires = DateTime.Now.AddHours(-50);
                Response.Cookies.Add(ck);
            }

            HttpCookie ck1 = new HttpCookie("firstname", (model.FirstName + "  " + model.LastName).ToString());
            ck1.Expires = DateTime.Now.AddHours(48);
            Response.Cookies.Add(ck1);

            if (resultUpdate)
                ViewBag.Result = "Cập nhật thông tin thành công.";
            else
                ViewBag.Result = "Có lỗi xảy ra trong quá trình cập nhật.";

            return View(model);
        }

        //start function login use FB,GG..

        #region

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult SignUpFacebook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpFacebook(DTO_Users_Acc model)
        {
            var objId = new ObjectId();

            UserLogin u = new UserLogin
            {
                _id = objId.ToString(),
            };
            Session.Add(Constants.USER_SESSION, u);

            HttpCookie ck1 = new HttpCookie("firstname", ("Facebook").ToString());
            ck1.Expires = DateTime.Now.AddHours(48);
            Response.Cookies.Add(ck1);
            ViewBag.SuccessMessage = "Đăng nhập thành công";
            Session.Remove(Constants.SESSION_CREDENTIALS);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult FacebookCallback(string code)
        {
            if (code != null)
            {
                var objId = new ObjectId();

                UserLogin u = new UserLogin
                {
                    _id = objId.ToString(),
                };
                Session.Add(Constants.USER_SESSION, u);

                HttpCookie ck1 = new HttpCookie("firstname", ("Facebook").ToString());
                ck1.Expires = DateTime.Now.AddHours(48);
                Response.Cookies.Add(ck1);
                ViewBag.SuccessMessage = "Đăng nhập thành công";
                Session.Remove(Constants.SESSION_CREDENTIALS);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public JsonResult LoginGoogle(string googleACModel)
        {
            ViewBag.GoogleSignIn = ConfigurationManager.AppSettings["GgAppId"].ToString();
            var accountSocialsList = new JavaScriptSerializer().Deserialize<List<AccountSocial>>(googleACModel);
            var accountSocials = accountSocialsList.FirstOrDefault();
            var memberAccount = new DTO_Users_Acc();
            memberAccount.Email = accountSocials.Email;
            memberAccount.FirstName = accountSocials.Email;
            memberAccount.LastName = accountSocials.FullName;
            var resultInsert = InsertByGoogle(memberAccount);

            UserLogin u = new UserLogin
            {
                _id = resultInsert.ToString(),
                LastName = accountSocials.FullName,
                Email = accountSocials.Email,
                Password = ""
            };
            //Save token API
            string token = GetToken(u.Email);
            HttpCookie cookie = new HttpCookie(Constants.TOKEN_NUMBER);
            HttpContext.Response.Cookies.Remove(Constants.TOKEN_NUMBER);
            cookie.Expires = DateTime.Now.AddDays(1);
            cookie.Value = token;
            HttpContext.Response.SetCookie(cookie);

            //Save session
            Session.Remove(Constants.USER_SESSION);
            Session.Add(Constants.USER_SESSION, u);

            //Save cookies
            HttpCookie ckUserAccount = new HttpCookie("usernameCustomer");
            ckUserAccount.Expires = DateTime.Now.AddHours(48);
            ckUserAccount.Value = u.Email;
            Response.Cookies.Add(ckUserAccount);

            HttpCookie ckIDAccount = new HttpCookie("idCustomer");
            ckIDAccount.Expires = DateTime.Now.AddHours(48);
            ckIDAccount.Value = u._id + "";
            Response.Cookies.Add(ckIDAccount);

            HttpCookie ckNameAccount = new HttpCookie("nameCustomer");
            ckNameAccount.Expires = DateTime.Now.AddHours(48);
            ckNameAccount.Value = u.LastName;
            Response.Cookies.Add(ckNameAccount);

            return Json(new { status = true });
        }

        public string InsertByFacebook(DTO_Users_Acc customer)
        {
            HttpResponseMessage response = serviceObj.PostResponse(url + "InsertForFacebook/", customer);
            response.EnsureSuccessStatusCode();
            string resultInsert = response.Content.ReadAsAsync<string>().Result;
            return resultInsert;
        }

        public int InsertByGoogle(DTO_Users_Acc customer)
        {
            HttpResponseMessage response = serviceObj.PostResponse(url + "InsertForGoogle/", customer);
            response.EnsureSuccessStatusCode();
            int resultInsert = response.Content.ReadAsAsync<int>().Result;
            return resultInsert;
        }
    }
}

#endregion