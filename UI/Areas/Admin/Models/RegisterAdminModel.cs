using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Areas.Admin.Models
{
    public class RegisterAdminModel
    {

        public string _id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Độ dài của tên tối thiểu là 3 chữ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập họ")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Độ dài của họ tối thiểu là 3 chữ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập email")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập email thực.")]

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Độ dài của tên tối thiểu là 3 chữ")]
        public string MerchantName { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu tối thiểu là 6 chữ số")]

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng.")]
        public string ConfirmPassword { get; set; }

        public int RoleId { get; set; }
    }
}