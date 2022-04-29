using DataAndServices.Admin_Services.CodeDiscountService;
using DataAndServices.DataModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace H_Shop.NetCore.Controllers.API_Admin
{
    [Route("api/CodeDiscount")]
    [ApiController]
    public class CodeDiscountController : ControllerBase
    {
        private readonly ICodeDiscountService _codeDiscountService;

        public CodeDiscountController(ICodeDiscountService codeDiscountService)
        {
            _codeDiscountService = codeDiscountService;
        } 

        [HttpPost]
        [Route("Create")]
        public async Task<bool> Create(CodeDiscount request)
        {
            return await _codeDiscountService.CreateCodeDiscount(request);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<bool> Update(CodeDiscount request)
        {
            return await _codeDiscountService.UpdateCodeDiscount(request);
        }

        [HttpDelete]
        [Route("Delete{id}")]
        public async Task<bool> DeleteAccount(string id)
        {
            return await _codeDiscountService.DeleteCodeDiscount(id);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var listAccount = await _codeDiscountService.GetAllCodeDiscount();
            return Ok(listAccount);
        }


        [HttpGet]
        [Route("GetById/{Id}")]
        public async Task<IActionResult> GetProductById(string Id)
        {
            var accId = await _codeDiscountService.GetCodeDiscount(Id);
            return Ok(accId);
        }
    }
}
