using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.CheckoutOrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IMongoCollection<Checkout_Oder> _db;
        public OrderService(DataContext db)
        {
            _db = db.GetCheckout_OderCollection();
        }
        public async Task<bool> DeleteAccount(string id)
        {

            try
            {
                await _db.DeleteOneAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<Checkout_Oder> GetAccountById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Checkout_Oder> GetAccountByIdKH(string id)
        {
            throw new NotImplementedException();
        }

        public List<Checkout_Oder> GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public Task<List<Checkout_Oder>> GetListAccountById(string id)
        {
            throw new NotImplementedException();
        }

        public bool Update_Ad_acc(Checkout_Oder dTO_Account)
        {
            throw new NotImplementedException();
        }

        //public async Task<Checkout_Oder> GetAccountById(string id)
        //{
        //    return await _db.Find(s => s._id == id).FirstOrDefaultAsync();

        //}

        //public async Task<Checkout_Oder> GetAccountByIdKH(string id)
        //{
        //    var results = _db.Find(x => x.TenSP == id).FirstOrDefaultAsync();
        //    var filter = Builders<Checkout_Oder>.Filter.Eq(x => x.TenSP, id);

        //    FilterDefinitionBuilder<Checkout_Oder> filter1 = Builders<Checkout_Oder>.Filter;
        //    FilterDefinition<Checkout_Oder> eqFilter = filter1.Eq(x => x.TenSP, id);

        //    var test = await _db.Find(filter).FirstOrDefaultAsync();
        //    return test;
        //}

        //public List<Checkout_Oder> GetAllAccounts()
        //{
        //    return  _db.Find(s => true).ToList();
        //}

        //public async Task<List<Checkout_Oder>> GetListAccountById(string id)
        //{
        //    return await _db.Find(s => s._id == id).ToListAsync();
        //}

        //public bool Update_Ad_acc(Checkout_Oder custom)
        //{
        //    var acc = GetAccountById(custom._id);
        //    if (acc != null)
        //    {
        //        var eqfilter = Builders<Checkout_Oder>.Filter.Where(s => s._id == custom._id);

        //        var update = Builders<Checkout_Oder>.Update.Set(s => s.TenSP, custom.TenSP)
        //            .Set(s => s.SoLuong, custom.SoLuong)
        //            .Set(s => s.Gia, custom.Gia)
        //            .Set(s => s._id, custom._id)
        //            //.Set(s => s.Id_KH, custom.Id_KH)

        //            .Set(s => s.TrangThai, custom.TrangThai);

        //        var options = new UpdateOptions { IsUpsert = true };




        //        _db.UpdateOneAsync(eqfilter, update, options);
        //        return true;
        //    }
        //    return false;
        //}
    }
}
