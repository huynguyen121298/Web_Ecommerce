using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DTO.DTO_Client
{
    public class DTO_Users_Acc
    {
        public string _id { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }    

        public string City { get; set; }

        public string Address { get; set; } 

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string FullName()
        {
            return this.FirstName + " " + this.LastName;
        }
        public int? RoleId { get; set; }
    }
}
