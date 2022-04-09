using DataAndServices.CommonModel;
using DataAndServices.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Client_Services
{
    public interface IProductClientServices
    {
        Task<List<Product>> GetAllProducts();

        List<Dis_Product> GetAllProductByPrice(int? gia, int? gia_);

        List<Dis_Product> GetAllProductByName(string name);

        Task<List<Product>> GetProductByMerchant(string merchantId);

        List<Account> GetMerchantByName(string merchantName);

        int GetSoLuong(string id);
    }
}
