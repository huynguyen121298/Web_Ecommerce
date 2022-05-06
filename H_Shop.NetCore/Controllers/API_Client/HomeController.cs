using DataAndServices.Client_Services;
using DataAndServices.DataModel;
using Microsoft.AspNetCore.Mvc;
using Model.DTO_Model;
using System.Threading.Tasks;

namespace H_Shop.NetCore.Controllers.API_Client
{
    [ApiController]
    [Route("api/Home")]    
    public class HomeController : ControllerBase
    {
        private readonly IHomeServices _homeServices;
        private readonly IProductClientServices _productClientServices; 
        public HomeController(IHomeServices homeServices,IProductClientServices productClientServices)
        {
            _homeServices = homeServices;
            _productClientServices = productClientServices;
        }

        [HttpGet]
        [Route("GetAllItemType")]
        public async Task<IActionResult> GetAllItemType()
        {
           var listItemType= await _homeServices.GetAllItemType();
            return Ok(listItemType);
        }

        [HttpGet]
        [Route("GetAllItemTypeUsed")]
        public async Task<IActionResult> GetAllItemTypeUsed()
        {
            var listItemType = await _homeServices.GetAllItemTypeUsed();
            return Ok(listItemType);
        }

        [HttpGet]
        [Route("getallfeedback")]
        public IActionResult GetAllFeedback()
        {
            var listItemType =  _homeServices.GetAllFeedbacks();
            return Ok(listItemType);
        }

        [HttpPost]
        [Route("Create")]
        public bool InsertFeedbacks(Feedback feedback)
        {
            return _homeServices.InsertFeedback( feedback);
            
        }

        [HttpPost]
        [Route("AddComment")]
        public bool InsertComment(ProductComment product)
        {
            return _productClientServices.InsertComment(product);

        }
    }
}
