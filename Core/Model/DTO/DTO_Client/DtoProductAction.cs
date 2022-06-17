namespace Model.DTO.DTO_Client
{
    public class DtoProductAction
    {
        public string _id { get; set; }

        public string UserId { get; set; }

        public string ProductId { get; set; }   

        /// <summary>
        /// 1 Bought / 2. Favorite
        /// </summary>
        public int Status { get; set; }

    }
}
