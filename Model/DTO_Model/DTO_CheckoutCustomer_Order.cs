using Model.DTO.DTO_Ad;
using System;
using System.Collections.Generic;

namespace Model.DTO_Model
{
    public class DTO_CheckoutCustomer_Order
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

        public Nullable<double> GiamGia { get; set; }

        public Nullable<double> TongTien { get; set; }

        public string TrangThai { get; set; }

        public int ID { get; set; }

        public List<DTO_Checkout_Order> dTO_Checkout_Orders { get; set; }
    }
}
