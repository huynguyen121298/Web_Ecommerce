using DataAndServices.CommonModel;
using DataAndServices.DataModel;
using Model.DTO_Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Client_Services
{
    public interface IProductClientServices
    {
        Task<List<Product>> GetAllProducts();

        List<Dis_Product> GetAllProductByPrice(int? gia, int? gia_);

        List<Dis_Product> GetAllProductByName(string name);

        List<Dis_Product> GetProductByMerchant(string merchantId);

        List<Account> GetMerchantByName(string merchantName);

        bool InsertComment(ProductComment product);

        bool DeleteProductAction(ProductAction product);

        bool InsertProductAction(List<ProductAction> productActions);

        int GetSoLuong(string id);
        List<Product> GetProductsBought(string userId);
        List<Product> GetProductsFavorite(string userId);
    }
}
