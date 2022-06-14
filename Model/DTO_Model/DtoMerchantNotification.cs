using System;

namespace Model.DTO_Model
{
    public class DtoMerchantNotification
    {
        public string _id { get; set; }

        public string AccountId { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public string CheckoutId { get; set; }

        public int Status { get; set; }

        public DateTime? DateTime { get; set; }
    }
}
