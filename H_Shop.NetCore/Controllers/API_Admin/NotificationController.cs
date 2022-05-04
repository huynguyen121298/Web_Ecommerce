using DataAndServices.Admin_Services.NotificationService;
using DataAndServices.DataModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace H_Shop.NetCore.Controllers.API_Admin
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("GetNotiByMerchant/{merchantId}")]
        public async Task<IActionResult> GetNotiByMerchant(string merchantId)
        {
            var listItemType = await _notificationService.GetMerchantNotification(merchantId);
            return Ok(listItemType);
        }

        [HttpPost]
        [Route("AddNotification")]
        public bool GetNotiByMerchant(List<MerchantNotification> request)
        {
            return _notificationService.AddNotication(request);
        }

        [HttpPut]
        [Route("ChangeStatus/{notiId}")]
        public MerchantNotification ChangeStatusNotication(string notiId)
        {
            return _notificationService.ChangeStatusNotification(notiId);
        }
    }
}
