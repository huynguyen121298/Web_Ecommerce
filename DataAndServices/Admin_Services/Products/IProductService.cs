using DataAndServices.CommonModel;
using DataAndServices.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.Products
{
    public interface IProductService
    {
        Task<List<Product_Admin>> GetAllProducts();

        Task<List<Product_Item_Type>> GetAllProductItem();

        List<List<Dis_Product>> GetAllProductItem_Type();

        Task<Product_Admin> GetProductById(string id);

        List<Dis_Product> GetProductById_Item(int id);

        Product_Item_Type GetProductItemById(string id);

        Product_Item_Type GetProductItemById_admin(string id);

        Product_Item_Type GetProductItemById2(string id);

        List<Product_Item_Type> GetProductItemByPageList();

        List<Product_Item_Type> GetProductItemById_client(int id);

        int CreateProduct(Product_Item_Type dTO_Account);

        bool DeleteAccount(string id);

        bool UpdateProduct(Product_Item_Type dTO_Account);

        Dis_Product GetProduct_DiscountById(string id);

        List<Dis_Product> GetAllProduct_Discount();

        bool InsertProduct_Discount(Discount_Product tO_Dis_Product);     
    }
}
