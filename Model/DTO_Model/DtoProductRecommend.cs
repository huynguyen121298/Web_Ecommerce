namespace Model.DTO_Model
{
    public class DtoProductRecommend
    {
        public string _id { get; set; }
        public int Frequency { get; set; }

        public string ProductId { get; set; }

        public string ItemTypeId { get; set; }

        public string UserId { get; set; }
    }
}
