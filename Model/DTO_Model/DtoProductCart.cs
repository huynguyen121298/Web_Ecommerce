using System.ComponentModel.DataAnnotations;

namespace Model.DTO_Model
{
    public class DtoProductCart
    {
        [Required(ErrorMessage = "Yêu cầu nhập mã sản phẩm")]
        public string _id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập số lượng")]
        public int? Quantity { get; set; }

        //[Required(ErrorMessage = "Yêu cầu nhập loại sản phẩm")]
        public string IdItemType { get; set; }

        public string Type_Product { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.PostalCode)]
        [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
        public int? Price { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập hình ảnh")]
        [StringLength(50)]
        public string Photo { get; set; }

        public string Photo2 { get; set; }

        public string Photo3 { get; set; }

        public string AccountId { get; set; }

        public string Details { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }
    }
}
