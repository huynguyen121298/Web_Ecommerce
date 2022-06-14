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
        private readonly IMongoCollection<Item> _dbItem;

        public ItemTypeService(DataContext db)
        {
            _dbItemType = db.GetItem_TypeCollection();
            _dbItem = db.GetItemCollection();
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

        public async Task<List<Item>> GetItem(string productId)
        {
            return await _dbItem.Find(i=>i.ProductId==productId).ToListAsync();
        }

        public async Task<Item> GetItemDetails(string itemId)
        {
            return await _dbItem.Find(i => i._id == itemId).FirstOrDefaultAsync();
        }



        public async Task<bool> UpdateItem(Item item)
        {
            var dbItem = GetItemDetails(item._id);
            if (dbItem != null)
            {
                var eqfilter = Builders<Item>.Filter.Where(s => s._id == item._id);

                var update = Builders<Item>.Update.Set(s => s.Quantity, item.Quantity)
                    .Set(s => s.Size, item.Size)
                    .Set(s=>s.Color, item.Color);

                var options = new UpdateOptions { IsUpsert = true };

                await _dbItem.UpdateOneAsync(eqfilter, update, options);
                return true;
            }
            return false;
        }

        public async Task<bool> CreateItem(Item request)
        {
            try
            {
                await _dbItem.InsertOneAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteItem(string itemId)
        {
            try
            {
                var eqfilter = Builders<Item>.Filter.Where(s => s._id == itemId);
                await _dbItem.DeleteOneAsync(eqfilter); 
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public async Task<string> GetItemByName(string itemName)
        {
            var item =await _dbItem.Find(s=>s.Color.StartsWith(itemName)).FirstOrDefaultAsync();
            return item._id;
        }
    }
}