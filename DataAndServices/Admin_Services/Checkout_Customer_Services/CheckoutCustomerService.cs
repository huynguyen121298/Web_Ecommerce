using AutoMapper;
using DataAndServices.Data;
using DataAndServices.DataModel;
using Model.DTO.DTO_Ad;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.Checkout_Customer_Services
{
    public class CheckoutCustomerService : ICheckoutCustomerService
    {
        private readonly IMongoCollection<CheckoutCustomerOrder> _db;
        private readonly IMapper _mapper;

        public CheckoutCustomerService(DataContext db,
            IMapper mapper)
        {
            _db = db.GetCheckoutCustomerOrderCollection();
            _mapper = mapper;
        }
        public bool DeleteAccount(string id)
        {
            try
            {
                var deleteFilter3 = Builders<CheckoutCustomerOrder>.Filter.Eq("_id", id);

                 _db.DeleteOne(deleteFilter3);

              
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<CheckoutCustomerOrder> GetAccountById(string id)
        {
            return await _db.Find(s => s._id == id).FirstOrDefaultAsync();
        }

        public List<CheckoutCustomerOrder> GetAllAccounts(string userLogin)
        
        {
            return  _db.Find(s=>s.AccountId== userLogin).ToList();
        }

        public IEnumerable<DTO_Checkout_Customer> GetMonthlyRevenue(int month)
        {
           
            var checkoutCustomers = _db.Find(s => s.TrangThai == "Hoàn Thành" && s.NgayTao.Month == month).ToList();

            var dtocheckoutCustomers = _mapper.Map<IEnumerable<CheckoutCustomerOrder>, IEnumerable<DTO_Checkout_Customer>>(checkoutCustomers);

            var monthTotal = dtocheckoutCustomers.Select(s=>s.TongTien).Sum();

            foreach (var dtocheckoutCustomer in dtocheckoutCustomers)
            {
                dtocheckoutCustomer.TongTienThang = monthTotal;
            }

            return dtocheckoutCustomers;
        }

        public IEnumerable<DTO_Checkout_Customer> GetYearRevenue(int year)
        {

            var checkoutCustomers = _db.Find(s => s.TrangThai == "Hoàn Thành" && s.NgayTao.Year == year).ToList();

            var dtocheckoutCustomers = _mapper.Map<IEnumerable<CheckoutCustomerOrder>, IEnumerable<DTO_Checkout_Customer>>(checkoutCustomers);

            var monthTotal = dtocheckoutCustomers.Select(s => s.TongTien).Sum();

            foreach (var dtocheckoutCustomer in dtocheckoutCustomers)
            {
                dtocheckoutCustomer.TongTienNam = monthTotal;
            }

            return dtocheckoutCustomers;
        }

        public async Task<List<CheckoutCustomerOrder>> GetListAccountById(string id)
        {
            return await _db.Find(s => s._id == id).ToListAsync();
        }

        public bool Update_Ad_acc(CheckoutCustomerOrder custom)
        {
            var acc = GetAccountById(custom._id);
            if (acc != null)
            {
                var eqfilter = Builders<CheckoutCustomerOrder>.Filter.Where(s => s._id == custom._id);

                var update = Builders<CheckoutCustomerOrder>.Update.Set(s => s.Email, custom.Email)
                    .Set(s => s.FirstName, custom.FirstName)
                    .Set(s => s.LastName, custom.LastName)
                    .Set(s => s.City, custom.City)
                    .Set(s => s.GiamGia, custom.GiamGia)
                    .Set(s => s.NgayTao, custom.NgayTao)
                    .Set(s => s.SDT, custom.SDT)
                    .Set(s => s.TongTien, custom.TongTien)
                    .Set(s => s.TrangThai, custom.TrangThai);

                var options = new UpdateOptions { IsUpsert = true };

                _db.UpdateOneAsync(eqfilter, update, options);
                return true;

            }
            return false;
        }
    }
}
