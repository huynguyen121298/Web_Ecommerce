using System.ComponentModel.DataAnnotations;

namespace Model.DTO.DTO_Ad
{
    public class DTO_Role
    {
        public int RoleId { get; set; }

        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(10)]
        public string Details { get; set; }

    }
}
