using AutoMapper;
using DataAndServices.Data;
using DataAndServices.DataModel;
using Model.DTO.DTO_Ad;
using Model.DTO_Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
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
            var ckCustomers =   _db.Find(s=>true).ToList();
            var ckCustomerOrders = new List<CheckoutCustomerOrder>();
            foreach(var c in ckCustomers)
            {
                var ckOrders = new List<Checkout_Oder>();
                foreach (var order in c.ProductOrder)
                {
                  
                    if(order.AccountId == userLogin)
                    {
                        ckOrders.Add(order);
                    }               
                }
                c.ProductOrder = ckOrders;
                ckCustomerOrders.Add(c);
            }
            return ckCustomerOrders;          
        }


        public static string[] SplitBitforBit(string text, int bitforbit)
        {
            int splitcount = Convert.ToInt32(RoundAt(text.Length / bitforbit, 0, 0));
            char[] allChars = text.ToCharArray();
            string[] splitted = new string[splitcount];
            int iL = 0;
            for (int i = 0; i != splitted.Length; i++)
            {
                splitted[i] = null;
                for (int j = 0; j != bitforbit; j++)
                {
                    splitted[i] += allChars[iL];
                    iL++;
                }
            }
            return splitted;
        }

        public static double RoundAt(double Number, int Position, int startUp)
        {
            double Up = Math.Abs(Number) * Math.Pow(10, Position);
            double temp = Up;
            double Out;
            while (Up > 0)
            {
                Up--;
            }
            Out = temp - Up;                           //Up
            if (Up < (Convert.ToDouble(startUp) - 10) / 10)
            { Out = temp - Up - 1; }                   //Down
            if (Number < 0)
            { Out *= -1; }
            Out /= Math.Pow(10, Position);
            return Out;
        }
        public DtoSalesVM GetMonthlyRevenue(string monthDate)
        {


            string[] splitted = SplitBitforBit(monthDate, 4);
            int year = Convert.ToInt32(splitted[0]);
            int month = Convert.ToInt32(splitted[1]);

           
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var days = Enumerable.Range(1, daysInMonth);
           
           
          

            var checkoutCustomers = _db.Find(s => s.TrangThai == "Hoàn Thành").ToList();


            var dtocheckoutCustomers = _mapper.Map<IEnumerable<CheckoutCustomerOrder>, IEnumerable<DTO_Checkout_Customer>>(checkoutCustomers);


            var test = dtocheckoutCustomers.Where(x => x.NgayTao.Year == year && x.NgayTao.Month == month).Select(g => new
            {
                Day = g.NgayTao.Day,
                Total = g.TongTien
            }); ;

            var monthTotal = new DtoSalesVM
            {
                Date = new DateTime(year, month, 1),
                Days = days.GroupJoin(test, d => d, q => q.Day, (d, q) => new DtoDayTotalVM
                {
                    Day = d,
                    Total = (decimal)q.Sum(x => x.Total)
                }).ToList()
            };
            monthTotal.Month = month;
            monthTotal.Year = year;

            //var monthTotal = dtocheckoutCustomers.Select(s=>s.TongTien).Sum();

            return monthTotal;
        }

        public double? GetDateRevenue(DateTime date)
        {

            var checkoutCustomers = _db.Find(s => s.TrangThai == "Hoàn Thành" && s.NgayTao == date ).ToList();

            var dtocheckoutCustomers = _mapper.Map<IEnumerable<CheckoutCustomerOrder>, IEnumerable<DTO_Checkout_Customer>>(checkoutCustomers);

            var yearTotal = dtocheckoutCustomers.Select(s => s.TongTien).Sum();

            //foreach (var dtocheckoutCustomer in dtocheckoutCustomers)
            //{
            //    dtocheckoutCustomer.TongTienNam = monthTotal;
            //}

            //return dtocheckoutCustomers;
            return yearTotal;
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
