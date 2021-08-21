using System.ComponentModel.DataAnnotations;

namespace DataAndServices.DataModel
{
    public class Checkout_Oder
    {

        public string Id_SanPham { get; set; }

        [StringLength(50)]
        public string TenSP { get; set; }

        public int? SoLuong { get; set; }

        public double? Gia { get; set; }

    }
}
