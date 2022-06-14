using DataAndServices.Admin_Services.Products;
using DataAndServices.CommonModel;
using DataAndServices.DataModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace H_Shop.NetCore.Controllers.API_Admin
{
    [ApiController]
    [Route("api/Products_Ad")]
    public class Products_AdController : ControllerBase
    {
        private readonly IProductService _productService;
        public Products_AdController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("GetProductById/{Id}")]
        public async Task<IActionResult> GetProductById(string Id)
        {
           var proById= await _productService.GetProductById(Id);
            return Ok(proById);
        }

        [HttpGet]
        [Route("GetProductItemById/{Id}")]
        public IActionResult GetProductItemById(string Id)
        {
           var proItemById=  _productService.GetProductItemById(Id);
            return Ok(proItemById);
        }

        [HttpGet]
        [Route("GetProductItemById_admin/{Id}")]
        public IActionResult GetProductItemById_Admin(string Id)
        {
            var proItemById=  _productService.GetProductItemById_admin(Id);
            return Ok(proItemById);
        }

        [HttpGet]
        [Route("GetAllProducts/{userLogin}")]
        public async Task<IActionResult> GetAllProducts(string userLogin)
        {
            var listPro= await _productService.GetAllProducts(userLogin);
            return Ok(listPro);
        }

        [HttpGet]
        [Route("GetAllProductsByEndUser")]
        public async Task<IActionResult> GetAllProductsByEndUser()
        {
            var listPro = await _productService.GetAllProductsByEndUser();
            return Ok(listPro);
        }

        [HttpGet]
        [Route("GetAllProduct/{userLogin}")]
        public async Task<IActionResult> GetAllProduct(string userLogin)
        {
            var listPro = await _productService.GetAllProductItem(userLogin);
            return Ok(listPro);
        }

        [HttpGet]
        [Route("GetAllProductByEndUser")]
        public async Task<IActionResult> GetAllProductByEndUser()
        {
            var listPro = await _productService.GetAllProductItemByEndUser();
            return Ok(listPro);
        }


        [HttpGet]
        [Route("GetAllProductByIdItem/{id}")]
        public IActionResult GetAllProductByIdItem(string id)
        {
           var listProItemById=  _productService.GetProductById_Item(id);
            return Ok(listProItemById);
        }

     
        [HttpGet]
        [Route("GetAllProductByType/{userLogin}")]
        public IActionResult GetAllProductByType(string userLogin)
        {
           var listlistProItem=  _productService.GetAllProductItem_Type(userLogin);
            return Ok(listlistProItem);
        }

        [HttpGet]
        [Route("GetAllProductByTypeByEndUser")]
        public IActionResult GetAllProductByTypeByEndUser()
        {
            var listlistProItem = _productService.GetAllProductItem_TypeByEndUser();
            return Ok(listlistProItem);
        }

        [HttpPost]
        [Route("CreateProduct")]
        public int CreateProduct(Product_Item_Type dTO_Product_Item)
        {
            return  _productService.CreateProduct(dTO_Product_Item);
        }

        [HttpPost]
        [Route("UpdateProduct")]
        public bool UpdateProduct(Product_Item_Type dTO_Product_Item)
        {
             return  _productService.UpdateProduct(dTO_Product_Item);
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public  bool DeleteProduct(string id)
        {
            return  _productService.DeleteAccount(id);
        }

        [HttpGet]
        [Route("GetAllProduct_Discount/{userLogin}")]
        public IActionResult GetAllProduct_Discount(string userLogin)
        {
            var listProDis=  _productService.GetAllProduct_Discount(userLogin);
            return Ok(listProDis);
        }

        [HttpGet]
        [Route("GetAllProduct_DiscountByEndUser")]
        public IActionResult GetAllProduct_Discount()
        {
            var listProDis = _productService.GetAllProduct_Discount();
            return Ok(listProDis);
        }

        [HttpGet]
        [Route("GetProduct_DiscountById/{id}")]
        public IActionResult GetProduct_DiscountById(string id)
        {
            var proDisById=  _productService.GetProduct_DiscountById(id);
            return Ok(proDisById);
        }

        [HttpPost]
        [Route("CreateProduct_Discount")]
        public bool CreateProduct_Discount(Discount_Product dTO_Product_Item)
        {
            return _productService.InsertProduct_Discount(dTO_Product_Item);
        }
    }
}
