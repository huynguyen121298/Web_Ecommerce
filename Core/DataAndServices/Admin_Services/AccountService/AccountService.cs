using DataAndServices.Common;
using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IMongoCollection<Account> _db;

        public AccountService(DataContext db)
        {
            _db = db.GetAccountCollection();
        }
        public bool AccountIsExits(string userName, string password)
        {
            var account = _db.FindSync(t => t.Email == userName).SingleOrDefault();
            if (account != null)
                return true;
            return false;
        }

        public async Task<bool> DeleteCustomer(string _id)
        {
            try
            {

                await _db.DeleteOneAsync(_id);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Account> GetCustomerByEmail(string email)
        {
            return await _db.Find(t => t.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Account> GetCustomerByID(string _id)
        {
            return await _db.Find(t => t._id == _id).FirstOrDefaultAsync();
        }
        public async Task<bool> UserNameIsExist(string userName)
        {
            var account = await _db.Find(t => t.Email == userName).SingleOrDefaultAsync();
            if (account == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> InsertCustomer(Account custom)
        {
            if (await UserNameIsExist(custom.Email) && custom != null)
            {
                try
                {
                    custom.Password = Encryptor.MD5Hash(custom.Password);
                    _db.InsertOne(custom);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<Account> LoginCustomer(string user, string pass)
        {
            string hash = Encryptor.MD5Hash(pass);
            var cus = await _db.Find(x => x.Email == user & x.Password == hash).FirstOrDefaultAsync();
            if (cus != null)
                return new Account() { _id = cus._id, Email = cus.Email, LastName = cus.LastName, FirstName = cus.FirstName, RoleId = cus.RoleId, MerchantName = cus.MerchantName, Photo = cus.Photo };
            return new Account();

        }

        public bool UpdateCustomer(Account custom)
        {
            try
            {

                var userAccount = GetCustomerByID(custom._id);
                if (userAccount != null)
                {
                    var eqfilter = Builders<Account>.Filter.Where(s => s._id == custom._id);

                    var update = Builders<Account>.Update.Set(s => s.Email, custom.Email)
                        .Set(s => s.FirstName, custom.FirstName)
                        .Set(s => s.LastName, custom.LastName)
                        .Set(s => s.Password, Encryptor.MD5Hash(custom.Password))
                        .Set(s => s._id, custom._id)
                        .Set(s => s.MerchantName, custom.MerchantName)
                        .Set(s=>s.Photo,custom.Photo);

                    var options = new UpdateOptions { IsUpsert = true };
                    _db.UpdateOneAsync(eqfilter, update, options);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
