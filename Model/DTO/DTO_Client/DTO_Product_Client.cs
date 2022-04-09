using System.ComponentModel.DataAnnotations;

namespace Model.DTO.DTO_Client
{
    public class DTO_Product
    {
        public string _id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? Price { get; set; }

        [StringLength(50)]
        public string Photo { get; set; }

        public string Details { get; set; }

        public int Id_Item { get; set; }

    }
}
