using DataAndServices.Client_Services;
using DataAndServices.DataModel;
using Microsoft.AspNetCore.Mvc;
using Model.DTO_Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace H_Shop.NetCore.Controllers.API_Client
{    
    [ApiController]
    [Route("api/Cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartServices;
        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        [HttpPost]
        [Route("InsertBill")]
        public List<DTOCheckoutCustomerOrder> InsertBill(CheckoutCustomerOrder checkoutCustomerOrder)
        {
            var idBill = _cartServices.InsertBill(checkoutCustomerOrder);
            return idBill;
        }

        [HttpPost]
        [Route("InsertCKOrder")]
        public bool InsertCKOrder(Checkout_Oder dTO_Account)
        {
            return  _cartServices.InsertCheckoutOrder(dTO_Account);
        }

        [HttpGet]
        [Route("GetGiamGia/{zipcode}")]
        public async Task< double> GetGiamGia(string zipcode)
        {
            return await _cartServices.GetGiamGia(zipcode);
        }
    }
}
