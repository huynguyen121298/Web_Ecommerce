namespace DataAndServices.CommonModel
{
    public class Account_Role
    {
        public string _id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName()
        {
            return this.FirstName + " " + this.LastName;
        }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Details { get; set; }
    }
}
