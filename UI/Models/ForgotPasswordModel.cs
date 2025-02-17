﻿using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Địa chỉ email vui lòng không để trống.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Vui lòng nhập email hợp lệ")]
        public string Email { get; set; }

        public bool SendReminderSuccess { get; set; } = false;
    }
}