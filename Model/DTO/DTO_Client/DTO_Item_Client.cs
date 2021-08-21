using System.Collections.Generic;
using System.Linq;

namespace Model.DTO.DTO_Client
{
    public class DTO_Item_Client
    {
         List<DTO_Item_Client> items = new List<DTO_Item_Client>();

        public string _id { get; set; }

        public int? Quantity { get; set; }

        public int Total_Quantity()
        {
            return (int)items.Sum(s => s.Quantity);
        }
    }
}
