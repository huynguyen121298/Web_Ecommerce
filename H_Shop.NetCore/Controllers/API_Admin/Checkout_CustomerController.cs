using DataAndServices.Admin_Services.Checkout_Customer_Services;
using DataAndServices.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H_Shop.NetCore.Controllers.API_Admin
{
    [Route("api/Checkout_Customer")]
    [ApiController]
    public class Checkout_CustomerController : ControllerBase
    {
        private readonly ICheckoutCustomerService _checkoutCustomerService;

        public Checkout_CustomerController(ICheckoutCustomerService checkoutCustomerService)
        {
            _checkoutCustomerService = checkoutCustomerService;
        }

        [HttpPost]
        [Route("Update")]
        public  bool Update(CheckoutCustomerOrder dTO_Account)
        {
            return _checkoutCustomerService.Update_Ad_acc(dTO_Account);
        }

        [HttpDelete]
        [Route("DeleteCustomer/{id}")]
        public bool DeleteCustomer(string id)
        {
            return  _checkoutCustomerService.DeleteAccount(id);
        }

        [HttpGet]
        [Route("GetAllCustomer")]
        public IActionResult GetAllCustomer()
        {   
             var listAccount=  _checkoutCustomerService.GetAllAccounts();
            return Ok(listAccount);

            
        }

        //[HttpGet]
        //[Route("GetAllAccounts2")]
        //public JsonResult<List<DTO_Account_Role>> GetAllAccounts2()
        //{
        //    return Json<List<DTO_Account_Role>>(_checkoutCustomerService.GetAllAccounts2());
        //}

        [HttpGet]
        [Route("GetCustomerById/{Id}")]
        public async Task<IActionResult> GetCustomerById(string Id)
        {
            var listAccount = await _checkoutCustomerService.GetAccountById(Id);
            return Ok(listAccount);  
        }

        [HttpGet]
        [Route("GetListCustomerById/{Id}")]
        public  async Task<IActionResult> GetListCustomerById(string Id)
        {
            var listAccount = await _checkoutCustomerService.GetListAccountById(Id);
        
            return Ok(listAccount);
            
        }
    }
}
