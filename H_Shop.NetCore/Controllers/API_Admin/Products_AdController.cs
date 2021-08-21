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
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var listPro= await _productService.GetAllProducts();
            return Ok(listPro);
        }

        [HttpGet]
        [Route("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var listPro = await _productService.GetAllProductItem();
            return Ok(listPro);
        }
 
        [HttpGet]
        [Route("GetAllProductByIdItem/{id}")]
        public IActionResult GetAllProductByIdItem(int id)
        {
           var listProItemById=  _productService.GetProductById_Item(id);
            return Ok(listProItemById);
        }

     
        [HttpGet]
        [Route("GetAllProductByType")]
        public IActionResult GetAllProductByType()
        {
           var listlistProItem=  _productService.GetAllProductItem_Type();
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
        [Route("GetAllProduct_Discount")]
        public IActionResult GetAllProduct_Discount()
        {
            var listProDis=  _productService.GetAllProduct_Discount();
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
