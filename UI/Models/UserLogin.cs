using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class UserLogin
    {
        [Key, Column(Order = 1)]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string _id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]

        // [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Mật khẩu")]

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }
        public bool RememberMe { set; get; }

        public string FullName()
        {
            return this.FirstName + " " + this.LastName;
        }
        public int? RoleId { get; set; }
    }
}