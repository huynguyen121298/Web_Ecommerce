using DataAndServices.Client_Services;
using Microsoft.AspNetCore.Mvc;

namespace H_Shop.NetCore.Controllers.API_Client
{
    [ApiController]
    [Route("api/Merchant")]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _merchantService;
        public MerchantController(IMerchantService merchantService)
        {
           _merchantService =   merchantService;
        }

   
        [HttpGet]
        [Route("GetAllProductByPrice/{giaMin:int}/{giaMax:int}/{merchantId}")]
        public IActionResult GetAllProductByPrice(int giaMin, int giaMax, string merchantId)
        {
            var listProByPrice = _merchantService.GetAllProductByPrice(giaMin, giaMax,merchantId);
            return Ok(listProByPrice);
        }

        [HttpGet]
        [Route("GetAllProductByName/{name}/{merchantId}")]
        public IActionResult GetAllProductByName(string name, string merchantId)
        {
            var proByName = _merchantService.GetAllProductByName(name,merchantId);
            return Ok(proByName);
        }

        [HttpGet]
        [Route("GetAllProductByIdItemClient/{id}/{merchantId}")]
        public IActionResult GetAllProductByIdItem(string id, string merchantId)
        {
            var proItemById = _merchantService.GetProductItemById_client(id, merchantId);
            return Ok(proItemById);

        }

        [HttpGet]
        [Route("GetAllProduct_DiscountByEndUser/{merchantId}")]
        public IActionResult GetAllProduct_Discount(string merchantId)
        {
            var listProDis = _merchantService.GetAllProduct_Discount(merchantId);
            return Ok(listProDis);
        }
    }
}
