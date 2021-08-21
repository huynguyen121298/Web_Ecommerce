using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class ResetPasswordModel2
    {
        public string _id { get; set; }
        [Required(ErrorMessage = "Mật khẩu mới không được để trống.", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }
        [Required(ErrorMessage = "Mật khẩu cũ không được để trống.", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không trùng khớp.")]
        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống.", AllowEmptyStrings = false)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetCode { get; set; }

        public string Mail { get; set; }
        public ResetPasswordModel2() { }
    }
}