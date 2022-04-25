using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.DTO.DTO_Ad
{
    public class DTO_Product
    {
        public DTO_Product()
        {
            Photo = "~/images_product/ap.jpg";
        }
        
        public string _id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? Price { get; set; }

        [StringLength(50)]
        public string Photo { get; set; }

        public string Details { get; set; }

    }
}
