using DataAndServices.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.CodeDiscountService
{
    public interface ICodeDiscountService
    {
        Task<bool> CreateCodeDiscount(CodeDiscount request);

        Task<bool> UpdateCodeDiscount(CodeDiscount request);

        Task<List<CodeDiscount>> GetAllCodeDiscount();

        Task<CodeDiscount> GetCodeDiscount(string codeId);  
        
        Task<bool> DeleteCodeDiscount(string codeId);
    }
}
