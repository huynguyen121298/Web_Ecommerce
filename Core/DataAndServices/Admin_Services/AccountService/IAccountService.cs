using DataAndServices.DataModel;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.AccountService
{
    public interface IAccountService
    {
        bool UpdateCustomer(Account cusUpdate);

        Task<bool> DeleteCustomer(string _id);

        Task<bool> InsertCustomer(Account cusInsert);

        Task<Account> GetCustomerByID(string _id);

        Task<Account> GetCustomerByEmail(string email);

        Task<Account> LoginCustomer(string user, string pass);
    }
}
