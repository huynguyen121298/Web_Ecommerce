using DataAndServices.Common;
using DataAndServices.CommonModel;
using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.Admin_Acc_Services
{
    public class AdminAccService : IAdminAccService
    {
        private readonly IMongoCollection<Account> _db;
        private readonly DataContext db = new DataContext("mongodb://localhost:27017", "OnlineShop");

        public AdminAccService(DataContext db)
        {
            _db = db.GetAccountCollection();
        }
        public bool Create_Ad_acc(Account account)
        {
            try
            {
                _db.InsertOne(account);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAccount(string id)
        {
            try
            {
                var deleteFilter3 = Builders<Account>.Filter.Eq("_id", id);
                await _db.DeleteOneAsync(deleteFilter3);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Account> GetAccountById(string id)
        {
            return await _db.Find(s => s._id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Account>> GetAllAccounts()
        {
            return await _db.Find(s => true).ToListAsync();
        }

        public List<Account_Role> GetAllAccounts2()
        {

            var acccountCollection = db.GetAccountCollection();
            var roleCollection = db.GetRoleCollection();
            var accountInfo = from account in acccountCollection.AsQueryable()
                              join role in roleCollection.AsQueryable() on account.RoleId equals role.RoleId
                              select new Account_Role()
                              {
                                  _id = account._id,
                                  FirstName = account.FirstName,
                                  LastName = account.LastName,
                                  RoleName = role.RoleName
                              };
            return accountInfo.ToList();
        }

        public bool Update_Ad_acc(Account custom)
        {
            var acc = GetAccountById(custom._id);
            if (acc != null)
            {
                var eqfilter = Builders<Account>.Filter.Where(s => s._id == custom._id);

                var update = Builders<Account>.Update.Set(s => s.Email, custom.Email)
                    .Set(s => s.FirstName, custom.FirstName)
                    .Set(s => s.LastName, custom.LastName)
                    .Set(s => s.Password, Encryptor.MD5Hash(custom.Password))
                    .Set(s => s._id, custom._id)
                    .Set(s => s.MerchantName, custom.MerchantName);

                var options = new UpdateOptions { IsUpsert = true };

                _db.UpdateOneAsync(eqfilter, update, options);
                return true;
            }
            return false;
        }

        public bool Update_Ad_acc2(Account custom)
        {
            var acc = GetAccountById(custom._id);
            if (acc != null)
            {
                var eqfilter = Builders<Account>.Filter.Where(s => s._id == custom._id);

                var update = Builders<Account>.Update.Set(s => s.Email, custom.Email)
                    .Set(s => s.FirstName, custom.FirstName)
                    .Set(s => s.LastName, custom.LastName)
                    .Set(s => s._id, custom._id);

                var options = new UpdateOptions { IsUpsert = true };

                _db.UpdateOneAsync(eqfilter, update, options);
                return true;
            }
            return false;
        }
    }
}
