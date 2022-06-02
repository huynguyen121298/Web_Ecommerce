namespace DataAndServices.DataModel
{
    public class ProductRecommend
    {
        public string _id { get; set; }

        public int Frequency { get; set; }

        public string ProductId { get; set; }

        public string ItemTypeId { get; set; }

        public string UserId { get; set; }
    }
}
