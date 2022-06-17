using DataAndServices.DataModel;
using Model.DTO_Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.Checkout_Customer_Services
{
    public interface ICheckoutCustomerService
    {
        List<CheckoutCustomerOrder> GetAllAccounts(string userLogin);

        Task<CheckoutCustomerOrder> GetAccountById(string id, string userLogin);

        Task<CheckoutCustomerOrder> GetAccountById2(string id);

        Task<List<CheckoutCustomerOrder>> GetListAccountById(string id);

        DtoSalesVM GetMonthlyRevenue(string month, string merchantId);

        double? GetDateRevenue(string merchantId);

        bool Update_Ad_acc(CheckoutCustomerOrder dTO_Account);

        Task<bool> DeleteAccount(string id);
    }
}



