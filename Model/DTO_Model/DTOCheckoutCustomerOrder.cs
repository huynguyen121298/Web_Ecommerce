using Model.DTO.DTO_Ad;
using System;
using System.Collections.Generic;

namespace Model.DTO_Model
{
    public class DTOCheckoutCustomerOrder
    {
      
        public string _id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int SDT { get; set; }

        public string DiaChi { get; set; }

        public string City { get; set; }

        public string Zipcode { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GiamGia { get; set; }

        public double? TongTien { get; set; }

        public double ? TongTienThang { get; set; }

        public double? TongTienNam { get; set; }

        public string TrangThai { get; set; }

        public IList<DTO_Checkout_Order> ProductOrder { get; set; }
    }
}
