using DataAndServices.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.UserServices
{
    public interface IUsers
    {
        Task<List<User_Acc>> GetAllAccounts();

        Task<User_Acc> GetAccountById(string id);

        Task<bool> Update_Ad_acc(User_Acc Account);

        bool Update_Ad_acc2(User_Acc Account);

        Task<bool> DeleteAccount(string id);      
    }
}
