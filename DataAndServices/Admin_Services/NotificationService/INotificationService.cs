using DataAndServices.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.NotificationService
{
    public interface INotificationService
    {
        bool AddNotication(List<MerchantNotification> request);

        Task<List<MerchantNotification>> GetMerchantNotification(string merchantId);

        MerchantNotification ChangeStatusNotification(string merchantId);

    }
}
