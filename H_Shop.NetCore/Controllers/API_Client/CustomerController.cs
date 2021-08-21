using DataAndServices.Client_Services;
using DataAndServices.DataModel;
using H_Shop.NetCore.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace H_Shop.NetCore.Controllers.API_Client
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string Secret = "ThisIsHuySuperSuperSecretKey1hai3bon5sau7tam";
        private readonly IHomeServices _homeServices;
        public CustomerController (IHomeServices homeServices)
        {
            _homeServices = homeServices;
        }

        //[EnableCors(origins: "*", headers: "*", methods: "*")]

        //[HttpGet]
        //public HttpResponseMessage ValidLogin(string user, string pass)
        //{
        //    if (user == "admin" && pass == "admin")
        //        return Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(user));
        //    else
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not valid");
        //}
        //public string GetLoginResultByUsernamePassword(string user, string pass)
        //{
        //    try
        //    {
        //        log.Info("Successful to response login result.");
        //        if(_homeServices.LoginCustomer(user,pass)==1)
        //            return TokenManager.GenerateToken(user);
        //        return "0";
        //    }
        //    catch (Exception)
        //    {
        //        log.Error("Cannot response login result.");
        //        throw;
        //    }
        //}

        [HttpPut]
        [Route("UpdateCustomer")]
        public bool UpdateCustomer(User_Acc_Client model)
        {
            try
            {
                return  _homeServices.UpdateCustomer(model);
            }
            catch (Exception)
            {
                log.Error("Cannot response result");
                throw;
            }
        }
        [HttpPut]
        [Route("UpdateCustomer2")]
        public bool UpdateCustomer2(User_Acc_Client model)
        {
            try
            {
                return  _homeServices.UpdateCustomer2(model);
            }
            catch (Exception)
            {
                log.Error("Cannot response result");
                throw;
            }
        }
        [HttpPut]
        [Route("UpdateCustomer3")]
        public bool UpdateCustomer3(User_Acc_Client model)
        {
            try
            {
                return  _homeServices.UpdateCustomer3(model);
            }
            catch (Exception)
            {
                log.Error("Cannot response result");
                throw;
            }
        }

        [HttpPost]
        [Route("InsertForFacebook")]
        public string InsertForFacebook(User_Acc_Client model)
        {
            try
            {
                return _homeServices.InsertForFacebook(model);
            }
            catch (Exception)
            {
                log.Error("Cannot response result");
                throw;
            }
        }

        [HttpPost]
        [Route(" InsertForGoogle")]
        public string InsertForGoogle(User_Acc_Client model)
        {
            try
            {
                return _homeServices.InsertForGoogle(model);
            }
            catch (Exception)
            {
                log.Error("Cannot response result");
                throw;
            }
        }

        [HttpPost]
        [Route("InsertCustomer")]
        public bool InsertCustomer(User_Acc_Client cusInsert)
        {
            try
            {
                return _homeServices.InsertCustomer(cusInsert);
            }
            catch (Exception)
            {
                log.Error("Cannot response result");
                throw;
            }
        }
        //[HttpGet]
        //public string Validate(string token, string username)
        //{
        //    if (account_BLL.UserNameIsExitst(username)) return "Invalid User";
        //    string tokenUsername = TokenManager.ValidateToken(token);
        //    if (username.Equals(tokenUsername))
        //    {
        //        return "Valid token";
        //    }
        //    else
        //        return "Invalid token";
        //}

        #region Get information
        [HttpGet]
        [Route("GetToken")]
        public  string GetToken(string username)
        {
            try
            {
                var token = TokenManager.GenerateToken(username);
                return token;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("GetCustomerByToken")]
        public async Task<User_Acc_Client> GetCustomerByToken(string token)
        {
            try
            {
                string tokenUsername = TokenManager.ValidateToken(token);
                log.Info("Get token API successful.");
                return await _homeServices.GetCustomerByUsername(tokenUsername);
            }
            catch (Exception ex)
            {
                log.Error("Cannot get customer by token api " + ex);
                throw;
            }
        }

        [HttpGet]
        [Route("GetCustomerByUsername")]
        public async Task<User_Acc_Client> GetCustomerByUsername(string user)
        {
            try
            {
                //string tokenUsername = TokenManager.ValidateToken(token);
                //if (user.Equals(tokenUsername))
                return await _homeServices.GetCustomerByUsername(user);
                //return null;
            }
            catch (Exception ex)
            {
                log.Error("Cannot response result." + ex);
                throw;
            }
        }

        [HttpGet]
        [Route("GetCustomerByEmail")]
        public async Task<User_Acc_Client> GetCustomerByEmail(string mail)
        {
            try
            {
                return await _homeServices.GetCustomerByEmail(mail);
            }
            catch (Exception)
            {
                log.Error("Cannot response result.");
                throw;
            }
        }

        [HttpGet]
        [Route("GetCustomerByPassword")]
        public string GetCustomerByPassword(string email)
        {
            try
            {
                return  _homeServices.GetCustomerByPassword(email);
            }
            catch (Exception)
            {
                log.Error("Cannot response result.");
                throw;
            }
        }

        [HttpGet]
        [Route("GetLoginResultByUsernamePassword")]
        public async Task<User_Acc_Client> GetLoginResultByUsernamePassword(string user, string pass)
        {
            try
            {
                log.Info("Successful to response login result.");
                return await _homeServices.LoginCustomer(user, pass);
            }
            catch (Exception)
            {
                log.Error("Cannot response login result.");
                throw;
            }
        }
        #endregion
    }
}
