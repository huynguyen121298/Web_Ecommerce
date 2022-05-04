using DataAndServices.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.ItemTypeService
{
    public interface IItemTypeService
    {
        Task<bool> CreateItemType(Item_type request);

        Task<bool> UpdateItemType(Item_type request);

        Task<List<Item_type>> GetAllItemType();

        Task<Item_type> GetItemType(string codeId);  
        
    }
}
