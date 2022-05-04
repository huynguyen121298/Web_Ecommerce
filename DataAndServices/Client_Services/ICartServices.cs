using DataAndServices.DataModel;
using System.Threading.Tasks;

namespace DataAndServices.Client_Services
{
    public interface ICartServices
    {
        string InsertBill(CheckoutCustomerOrder checkoutCustomerOrder);

        bool InsertCheckoutOrder(Checkout_Oder checkout_Order);

        Task<double> GetGiamGia(string zipcode);      
    }
}
