﻿using DataAndServices.Common;
using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
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
            var merchantNotis = await _db.Find(n => n.AccountId == merchantId).ToListAsync();
            var orderByDescendingResult = (from noti in merchantNotis
                                           orderby noti.DateTime descending
                                           select noti).ToList();
            return orderByDescendingResult;
        }

        public MerchantNotification ChangeStatusNotification(string notiId)
        {

            var acc =  _db.Find(s => s._id == notiId && s.Status == NotificationConstant.PENDING).FirstOrDefaultAsync();

            if (acc != null)
            {
                var eqfilter = Builders<MerchantNotification>.Filter.Where(s => s._id == notiId && s.Status == NotificationConstant.PENDING);

                var update = Builders<MerchantNotification>.Update.Set(s => s.Status, NotificationConstant.READED);

                var options = new UpdateOptions { IsUpsert = true };

                _db.UpdateOneAsync(eqfilter, update, options);
                var notiUpdated = _db.Find(n => n._id == notiId).FirstOrDefault();
                return notiUpdated;

            }
            return null;
           

               
            
        }
    }
}
