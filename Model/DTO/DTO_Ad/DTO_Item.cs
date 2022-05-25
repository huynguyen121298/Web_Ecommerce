using System.Collections.Generic;

namespace Model.DTO.DTO_Ad
{
    public class DTO_Item
    {
        public string _id { get; set; }

        public int? Quantity { get; set; }

        public List<string> Color { get; set; }

        public List<string> Size { get; set; }
    }
}
