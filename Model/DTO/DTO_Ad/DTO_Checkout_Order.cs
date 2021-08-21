using System.ComponentModel.DataAnnotations;

namespace Model.DTO.DTO_Ad
{
    public class DTO_Checkout_Order
    {
      
        public string Id_SanPham { get; set; }

        [StringLength(50)]
        public string TenSP { get; set; }

        public int? SoLuong { get; set; }

        public double? Gia { get; set; }
    }
}
