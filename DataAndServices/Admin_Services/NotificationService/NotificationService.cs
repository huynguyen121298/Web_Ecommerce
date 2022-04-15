using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IMongoCollection<MerchantNotification> _db;       

        public NotificationService(DataContext db)
        {
            _db = db.GetMerchantNotificationCollection();
        }
        public bool AddNotication(List<MerchantNotification> request)
        {
            try
            {
                _db.InsertMany(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<MerchantNotification>> GetMerchantNotification(string merchantId)
        {
            var merchantNotis = await _db.FindAsync(n => n.AccountId == merchantId);
            return merchantNotis.ToList();
        }
    }
}
