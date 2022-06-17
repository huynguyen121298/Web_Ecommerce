using DataAndServices.CommonModel;
using System.Collections.Generic;

namespace DataAndServices.Client_Services
{
    public interface IMerchantService
    {
        List<Dis_Product> GetAllProductByName(string name, string merchantId);
        List<Dis_Product> GetAllProductByPrice(int giaMin, int giaMax, string merchantId);
        List<Product_Item_Type> GetProductItemById_client(string id, string merchantId);
        List<Dis_Product> GetAllProduct_Discount(string merchantId);
    }
}
