using DataAndServices.DataModel;
using System.Threading.Tasks;

namespace DataAndServices.Client_Services
{
    public interface ICartServices
    {
        int InsertBill(CheckoutCustomerOrder checkoutCustomerOrder);

        bool InsertCheckoutOrder(Checkout_Oder checkout_Order);

        Task<double> GetGiamGia(string zipcode);      
    }
}
