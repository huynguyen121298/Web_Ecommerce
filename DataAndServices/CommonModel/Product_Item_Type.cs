using DataAndServices.DataModel;
using System.Collections.Generic;

namespace DataAndServices.CommonModel
{
    public class Product_Item_Type
    {
        public string _id { get; set; }

        public int? Quantity { get; set; }

        public string IdItemType { get; set; }

        public string Type_Product { get; set; }

        public string Name { get; set; }

        public int? Price { get; set; }

        public string Photo { get; set; }

        public string Photo2 { get; set; }

        public string Photo3 { get; set; }

        public string AccountId { get; set; }

        public string Details { get; set; }

        public List<ProductComment> Comments { get; set; }

        public List<Item> Items { get; set; }

        public int Rating { get; set; }



    }
}
