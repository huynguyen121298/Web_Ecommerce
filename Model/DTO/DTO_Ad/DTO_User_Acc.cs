using System.ComponentModel.DataAnnotations;

namespace Model.DTO.DTO_Ad
{
    public class DTO_User_Acc
    {
        public string _id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        //[Required]
        //[StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //1 : default , 2 facebook
        public int? RoleId { get; set; }

        public string PhoneNumber { get; set; }

        public string City { get; set; }

        public string Address { get; set; }
    }
}
