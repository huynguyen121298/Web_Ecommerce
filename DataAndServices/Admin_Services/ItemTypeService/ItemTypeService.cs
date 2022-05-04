using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.ItemTypeService
{
    public class ItemTypeService : IItemTypeService
    {
        private readonly IMongoCollection<Item_type> _dbItemType;

        public ItemTypeService(DataContext db)
        {
            _dbItemType = db.GetItem_TypeCollection();
        }
        public async Task<bool> CreateItemType(Item_type request)
        {
            try
            {
                request.Status = "Đã kích hoạt";
               await _dbItemType.InsertOneAsync(request);
               return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Item_type>> GetAllItemType()
        {
            var codeDiscounts = await _dbItemType.Find(c => true).ToListAsync();
            return codeDiscounts;
        }

        public async Task<Item_type> GetItemType(string codeId)
        {
            var codeDiscount = await _dbItemType.Find(s => s._id == codeId).FirstOrDefaultAsync(); ;
            return codeDiscount;
        }

        public async Task<bool> UpdateItemType(Item_type request)
        {
            var acc = GetItemType(request._id);
            if (acc != null)
            {
                var eqfilter = Builders<Item_type>.Filter.Where(s => s._id == request._id);

                var update = Builders<Item_type>.Update.Set(s => s.Type_Product, request.Type_Product)
                    .Set(s => s.Status, request.Status);

                var options = new UpdateOptions { IsUpsert = true };

                await _dbItemType.UpdateOneAsync(eqfilter, update, options);
                return true;
            }
            return false;
        }


    }
}
