using DataAndServices.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.CheckoutOrderServices
{
    public interface IOrderService
    {
        List<Checkout_Oder> GetAllAccounts();



        Task<Checkout_Oder> GetAccountById(string id);

        Task<Checkout_Oder> GetAccountByIdKH(string id);



        Task<List<Checkout_Oder>> GetListAccountById(string id);



        bool Update_Ad_acc(Checkout_Oder dTO_Account);

        Task<bool> DeleteAccount(string id);
    }
}
