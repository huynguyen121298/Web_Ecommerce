using DataAndServices.CommonModel;
using DataAndServices.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.Admin_Acc_Services
{
    public interface IAdminAccService
    {
        Task<List<Account>> GetAllAccounts();

        List<Account_Role> GetAllAccounts2();

        Task<Account> GetAccountById(string id);

        bool Create_Ad_acc(Account Account);

        bool Update_Ad_acc(Account Account);

        bool Update_Ad_acc2(Account Account);

        Task<bool> DeleteAccount(string id);
      
    }
}
