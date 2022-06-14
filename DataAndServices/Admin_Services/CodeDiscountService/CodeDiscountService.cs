using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.CodeDiscountService
{
    public class CodeDiscountService : ICodeDiscountService
    {
        private readonly IMongoCollection<CodeDiscount> _dbCodeDiscount;

        public CodeDiscountService(DataContext db)
        {
            _dbCodeDiscount = db.GetCodeDiscountCollection();
        }
        public async Task<bool> CreateCodeDiscount(CodeDiscount request)
        {
            try
            {
               await _dbCodeDiscount.InsertOneAsync(request);
               return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCodeDiscount(string codeId)
        {
            try
            {
                var deleteFilter3 = Builders<CodeDiscount>.Filter.Eq("_id", codeId);
                await _dbCodeDiscount.DeleteOneAsync(deleteFilter3);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<CodeDiscount>> GetAllCodeDiscount()
        {
            var codeDiscounts = await _dbCodeDiscount.Find(c => true).ToListAsync();
            return codeDiscounts;
        }

        public async Task<CodeDiscount> GetCodeDiscount(string codeId)
        {
            var codeDiscount = await _dbCodeDiscount.Find(s => s._id == codeId).FirstOrDefaultAsync(); ;
            return codeDiscount;
        }

        public async Task<bool> UpdateCodeDiscount(CodeDiscount request)
        {
            var acc = GetCodeDiscount(request._id);
            if (acc != null)
            {
                var eqfilter = Builders<CodeDiscount>.Filter.Where(s => s._id == request._id);

                var update = Builders<CodeDiscount>.Update.Set(s => s.Zipcode, request.Zipcode)
                    .Set(s => s.Discount, request.Discount);

                var options = new UpdateOptions { IsUpsert = true };

                await _dbCodeDiscount.UpdateOneAsync(eqfilter, update, options);
                return true;
            }
            return false;
        }


    }
}
