using System;
using System.Collections.Generic;

namespace Model.DTO_Model
{
    public class DTO_Dis_Product
    {
        public string _id { get; set; }

        public string Content { get; set; }

        public Nullable<double> Price_Dis { get; set; }

        public Nullable<System.DateTime> Start { get; set; }

        public Nullable<System.DateTime> End { get; set; }      
        
        public string Name { get; set; }

        public Nullable<int> Price { get; set; }

        public string Photo { get; set; }

        public string Photo2 { get; set; }

        public string Photo3 { get; set; }

        public string Details { get; set; }

        public List<DtoProductComment> Comments { get; set; }

        public string AccountId { get; set; }

        public string IdItemType { get; set; }

        public string Type_Product { get; set; }
        
        public int Rating { get; set; }

        public string ItemId { get; set; }

    }
}
