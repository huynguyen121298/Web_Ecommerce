using DataAndServices.DataModel;
using Model.DTO.DTO_Ad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.Checkout_Customer_Services
{
    public interface ICheckoutCustomerService
    {
        List<CheckoutCustomerOrder> GetAllAccounts(string userLogin);

        Task<CheckoutCustomerOrder> GetAccountById(string id);

        Task<List<CheckoutCustomerOrder>> GetListAccountById(string id);

        IEnumerable<DTO_Checkout_Customer> GetMonthlyRevenue(int month);

        IEnumerable<DTO_Checkout_Customer> GetYearRevenue(int year);

        bool Update_Ad_acc(CheckoutCustomerOrder dTO_Account);

        bool DeleteAccount(string id);
    }
}



