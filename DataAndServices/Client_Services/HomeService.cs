using DataAndServices.Common;
using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAndServices.Client_Services
{
    public class HomeService : IHomeServices
    {
        private readonly IMongoCollection<User_Acc_Client> _dbUser;
        private readonly IMongoCollection<Feedback> _dbFeed;
        private readonly IMongoCollection<Item_type> _dbitemType;

        public HomeService(DataContext db)
        {
            _dbUser = db.GetUser_Acc_ClientCollection();
            _dbFeed = db.GetFeedbackCollection();
            _dbitemType = db.GetItem_TypeCollection();
        }
        public async Task<bool> DeleteCustomer(string id)
        {
            await _dbUser.DeleteOneAsync(id);
            return true;
        }

        public async Task<List<User_Acc_Client>> GetAllCustomers()
        {
            return await _dbUser.Find(s => true).ToListAsync();
        }

        public List<Feedback> GetAllFeedbacks()
        {
            return  _dbFeed.Find(s => true).ToList();
        }

        public async Task<List<Item_type>> GetAllItemType()
        {
            return await _dbitemType.Find(s => true).ToListAsync();
        }

        public async Task<List<Item_type>> GetAllItemTypeUsed()
        {
            return await _dbitemType.Find(s =>s.Status =="Đã kích hoạt").ToListAsync();
        }

        public async Task<User_Acc_Client> GetCustomerByEmail(string mail)
        {
            return await _dbUser.Find(s => s.Email==mail).FirstOrDefaultAsync();
        }

        public async Task<User_Acc_Client> GetCustomerByID(string id)
        {
            return await _dbUser.Find(s => s._id == id).FirstOrDefaultAsync();
        }

        public string GetCustomerByPassword(string email)
        {
            string pass = Encryptor.MD5Hash(email);
            var account = _dbUser.Find(s => s.Password == pass).SingleOrDefault();

            if (account != null)
                return Encryptor.MD5Hash(account.Password);
            return "";
        }

        public async Task<User_Acc_Client> GetCustomerByUsername(string user)
        {
            return await _dbUser.Find(s => s.Email == user).FirstOrDefaultAsync();
        }

        public bool InsertCustomer(User_Acc_Client custom)
        {
            if (!UserNameIsExist(custom.Email) && custom != null)
            {
                try
                {
                    custom.Password = Encryptor.MD5Hash(custom.Password);
                    _dbUser.InsertOne(custom);
                 
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public bool InsertFeedback(Feedback custom)
        {
            try
            {
                _dbFeed.InsertOne(custom);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string PasswordIsExist(string email)
        {
            string pass = Encryptor.MD5Hash(email);
            var account = _dbUser.Find(t => t.Password == pass).SingleOrDefault();


            if (account != null)
                return Encryptor.MD5Hash(account.Password).ToString();
            return "";

        }
        public bool UserNameIsExist(string userName)
        {

            var account = _dbUser.Find(t => t.Email == userName).SingleOrDefault();
            if (account != null)
                return true;
            return false;
        }

        public string InsertForFacebook(User_Acc_Client cusInsert)
        {
            var user = _dbUser.Find(x => x.Email == cusInsert.Email).SingleOrDefault();
            if (user == null)
            {
                cusInsert.RoleId = 2;
                cusInsert.FirstName =cusInsert.Email;
                cusInsert.Password = Encryptor.MD5Hash(cusInsert.Email);

                _dbUser.InsertOne(cusInsert);

                return cusInsert._id;
            }
            else
            {
                return user._id;
            }
        }

        public string InsertForGoogle(User_Acc_Client cusInsert)
        {
            var user = _dbUser.Find(x => x.Email == cusInsert.Email).SingleOrDefault();
            if (user == null)
            {
                _dbUser.InsertOne(cusInsert);
                
                return cusInsert._id;
            }
            else
            {
                return user._id;
            }
            
        }

        public async Task<User_Acc_Client> LoginCustomer(string user, string pass)
        {
            string hash = Encryptor.MD5Hash(pass);
            var cus = await _dbUser.Find(x => x.Email == user & x.Password == hash).FirstOrDefaultAsync();
            if (cus != null)
                return new User_Acc_Client() { _id = cus._id, Email = cus.Email, LastName = cus.LastName, FirstName = cus.FirstName,RoleId = cus.RoleId };
            return new User_Acc_Client();

        }

        public bool UpdateCustomer(User_Acc_Client custom)
        {
            try
            {
                var userAccount = GetCustomerByID(custom._id);

                if (userAccount != null)
                {

                    var eqfilter = Builders<User_Acc_Client>.Filter.Where(s => s._id == custom._id);
                    var update = Builders<User_Acc_Client>.Update.Set(s => s.Email, custom.Email)
                        .Set(s => s.FirstName, custom.FirstName)
                        .Set(s => s.LastName, custom.LastName)
                        .Set(s => s._id, custom._id)
                        .Set(s => s.PhoneNumber, custom.PhoneNumber)
                        .Set(s => s.Address, custom.Address)
                        .Set(s => s.City, custom.City);
                    var options = new UpdateOptions { IsUpsert = true };

                    _dbUser.UpdateOneAsync(eqfilter, update, options);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateCustomer2(User_Acc_Client custom)
        {
            try
            {
                var userAccount = GetCustomerByID(custom._id);
                if (userAccount != null)
                {
                    var eqfilter = Builders<User_Acc_Client>.Filter.Where(s => s._id == custom._id);
                    var update = Builders<User_Acc_Client>.Update.Set(s => s.Email, custom.Email)
                        .Set(s => s.FirstName, custom.FirstName)
                        .Set(s => s.LastName, custom.LastName)
                        .Set(s => s.Password, Encryptor.MD5Hash(custom.Password))
                        .Set(s => s._id, custom._id);
                    var options = new UpdateOptions { IsUpsert = true };

                    _dbUser.UpdateOneAsync(eqfilter, update, options);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateCustomer3(User_Acc_Client custom)
        {
            try
            {
                var userAccount = GetCustomerByID(custom._id);

                if (userAccount != null)
                {
                   
                    var eqfilter = Builders<User_Acc_Client>.Filter.Where(s => s._id== custom._id);

                    var update = Builders<User_Acc_Client>.Update.Set(s => s.Email, custom.Email)
                        .Set(s => s.FirstName, custom.FirstName)
                        .Set(s => s.LastName, custom.LastName)
                        .Set(s => s.Password, Encryptor.MD5Hash(custom.Password))
                        .Set(s => s._id, custom._id);

                    var options = new UpdateOptions { IsUpsert = true };

                    _dbUser.UpdateOneAsync(eqfilter, update, options);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
