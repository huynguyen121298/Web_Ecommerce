using DataAndServices.DataModel;
using Model.DTO.DTO_Ad;
using Model.DTO_Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.Checkout_Customer_Services
{
    public interface ICheckoutCustomerService
    {
        List<CheckoutCustomerOrder> GetAllAccounts(string userLogin);

        Task<CheckoutCustomerOrder> GetAccountById(string id);

        Task<List<CheckoutCustomerOrder>> GetListAccountById(string id);

        DtoSalesVM GetMonthlyRevenue(string month);

        double? GetDateRevenue(DateTime date);

        bool Update_Ad_acc(CheckoutCustomerOrder dTO_Account);

        bool DeleteAccount(string id);
    }
}



