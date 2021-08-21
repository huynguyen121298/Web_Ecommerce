using DataAndServices.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Client_Services
{
    public interface IHomeServices
    {
        Task<List<User_Acc_Client>> GetAllCustomers();

        Task<List<Item_type>> GetAllItemType();

        List<Feedback> GetAllFeedbacks();

        bool InsertCustomer(User_Acc_Client cusInsert);

        string InsertForFacebook(User_Acc_Client cusInsert);

        string InsertForGoogle(User_Acc_Client cusInsert);

        bool UpdateCustomer(User_Acc_Client cusUpdate);

        bool UpdateCustomer2(User_Acc_Client cusUpdate);

        bool UpdateCustomer3(User_Acc_Client cusUpdate);

        Task<bool> DeleteCustomer(string id);

        Task<User_Acc_Client> GetCustomerByID(string id);

        Task<User_Acc_Client> GetCustomerByUsername(string user);

        Task<User_Acc_Client> GetCustomerByEmail(string mail);

        string GetCustomerByPassword(string email);

        Task<User_Acc_Client> LoginCustomer(string user, string pass);

        bool InsertFeedback(Feedback feedback);
    }
}
