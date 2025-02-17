﻿using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class LoginModel
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [StringLength(maximumLength: 500, MinimumLength = 5, ErrorMessage = "Tên đăng nhập nằm trong 5-500 kí tự")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [Display(Name = "Mật khẩu")]
        public string Password { set; get; }

        public bool RememberMe { set; get; }

        public string GroupID { set; get; }

        public LoginModel() { }
    }
}