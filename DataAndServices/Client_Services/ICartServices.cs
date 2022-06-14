using DataAndServices.DataModel;
using Model.DTO_Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Client_Services
{
    public interface ICartServices
    {
        List<DTOCheckoutCustomerOrder> InsertBill(CheckoutCustomerOrder checkoutCustomerOrder);

        bool InsertCheckoutOrder(Checkout_Oder checkout_Order);

        Task<double> GetGiamGia(string zipcode);      
    }
}
