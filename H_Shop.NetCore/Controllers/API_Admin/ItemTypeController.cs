using DataAndServices.Admin_Services.CodeDiscountService;
using DataAndServices.Admin_Services.ItemTypeService;
using DataAndServices.DataModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace H_Shop.NetCore.Controllers.API_Admin
{
    [Route("api/ItemType")]
    [ApiController]
    public class ItemTypeController : ControllerBase
    {
        private readonly IItemTypeService _itemTypeService;

        public ItemTypeController(IItemTypeService itemTypeService)
        {
            _itemTypeService = itemTypeService;
        } 

        [HttpPost]
        [Route("Create")]
        public async Task<bool> Create(Item_type request)
        {
            return await _itemTypeService.CreateItemType(request);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<bool> Update(Item_type request)
        {
            return await _itemTypeService.UpdateItemType(request);
        }

   

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var listAccount = await _itemTypeService.GetAllItemType();
            return Ok(listAccount);
        }


        [HttpGet]
        [Route("GetById/{Id}")]
        public async Task<IActionResult> GetProductById(string Id)
        {
            var accId = await _itemTypeService.GetItemType(Id);
            return Ok(accId);
        }

        [HttpGet]
        [Route("GetItem/{productId}")]
        public async Task<IActionResult> GetItems(string productId)
        {
            var listAccount = await _itemTypeService.GetItem(productId);
            return Ok(listAccount);
        }

        [HttpPost]
        [Route("UpdateItem")]
        public async Task<bool> UpdateItem(Item request)
        {
            return await _itemTypeService.UpdateItem(request);
        }

        [HttpGet]
        [Route("ItemDetails/{itemId}")]
        public async Task<IActionResult> GetItemDetails(string itemId)
        {
            var listAccount = await _itemTypeService.GetItemDetails(itemId);
            return Ok(listAccount);
        }
    }
}
