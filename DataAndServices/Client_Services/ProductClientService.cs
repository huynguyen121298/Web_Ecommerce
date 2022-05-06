using DataAndServices.CommonModel;
using DataAndServices.Data;
using DataAndServices.DataModel;
using Model.Common;
using Model.DTO_Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAndServices.Client_Services
{
    public class ProductClientService : IProductClientServices
    {
        private readonly IMongoCollection<Product> _db;
        private readonly IMongoCollection<Item> _dbItem;
        private readonly IMongoCollection<Discount_Product> _dbDis;
        private readonly IMongoCollection<Account> _dbAcc;
        private readonly IMongoCollection<ProductComment> _dbProductComment;
        private readonly IMongoCollection<ProductAction> _dbProductAction;
        //private DataContext db = new DataContext("mongodb://localhost:27017", "OnlineShop");
        public ProductClientService(DataContext db)
        {
            _db = db.GetProductClientCollection();
            _dbItem = db.GetItemCollection();
            _dbDis = db.GetDiscountProductCollection();
            _dbAcc = db.GetAccountCollection();
            _dbProductComment = db.GetProductCommentCollection();
            _dbProductAction = db.GetProductActionCollection();
        }
        public List<Dis_Product> GetAllProductByName(string name)
        {
            var discountCollection = _dbDis;
            var productCollection = _db;
            var Info = (from dis in discountCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on dis._id equals product._id

                        select new Dis_Product()
                        {
                            _id = dis._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            IdItemType = product.IdItemType,
                            Content = dis.Content,
                            Price_Dis = dis.Price_Dis,
                            Start = dis.Start,
                            End = dis.End
                        }).Where(s => s.Name.StartsWith(name)).ToList();
            return Info;
        }

        public List<Account> GetMerchantByName(string merchantName)
        {
            var merchantByName = _dbAcc.Find(s=>s.MerchantName.StartsWith(merchantName)).ToList();            
            return merchantByName;
        }

        public List<Dis_Product> GetAllProductByPrice(int? gia, int? gia_)
        {
            var discountCollection = _dbDis;
            var productCollection = _db;
            var Info = (from dis in discountCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on dis._id equals product._id

                        select new Dis_Product()
                        {
                            _id = dis._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            IdItemType = product.IdItemType,
                            Content = dis.Content,
                            Price_Dis = dis.Price_Dis,
                            Start = dis.Start,
                            End = dis.End
                        });
            List<Dis_Product> dis_Product = new List<Dis_Product>();
            List<Dis_Product> dis_Product2 = new List<Dis_Product>();
            Dis_Product dis_Product3 = new Dis_Product();
            Dis_Product dis_Product4 = new Dis_Product();
            List<Dis_Product> dis_Product5 = new List<Dis_Product>();
            List<Dis_Product> dis_Product6 = new List<Dis_Product>();

            foreach (var item in Info)
            {
                //dis_Product.Add(item);
                if (GetPriceDiscountById(item._id) != 0)
                {
                    //item.Price = Convert.ToInt32(GetPriceDiscountById((Convert.ToInt32(item.Id_SanPham))));

                    bool dis_Product7 = (item.Price_Dis <= gia_ && item.Price_Dis >= gia);
                    if (dis_Product7 == true)
                    {
                        dis_Product5.Add(item);
                    }

                }
                else
                {
                    bool dis_Product8 = (item.Price <= gia_ && item.Price >= gia);
                    if (dis_Product8 == true)
                    {
                        dis_Product2.Add(item);
                    }

                }
            }
            dis_Product6.AddRange(dis_Product2);
            dis_Product6.AddRange(dis_Product5);

            return dis_Product6;
        }
        public double GetPriceDiscountById(string id)
        {
            DateTime dateTime = DateTime.Today;
            var item_discount = _dbDis.Find(t => t._id == id && t.End.Value >= dateTime).FirstOrDefault();

            if (item_discount != null && item_discount.Price_Dis != null)
            {
                return Convert.ToDouble(item_discount.Price_Dis);
            }
            return 0;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _db.Find(s => true).ToListAsync();
        }


        public async Task<int> GetSoLuong(string id)
        {
            var temp = await _dbItem.Find(s => s._id == id).FirstOrDefaultAsync();
            if (temp != null)
            {
                return (int)temp.Quantity;
            }
            return 0;
        }

        public List<Dis_Product> GetProductByMerchant(string merchantId)
        {

            var discountCollection = _dbDis;
            var productCollection = _db;

            var Info = (from dis in discountCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on dis._id equals product._id
                        where product.AccountId == merchantId
                        select new Dis_Product()
                        {
                            _id = dis._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            IdItemType = product.IdItemType,
                            Content = dis.Content,
                            Price_Dis = dis.Price_Dis,
                            Start = dis.Start,
                            End = dis.End,
                            AccountId = product.AccountId
                        });

            return Info.ToList();
            //var productsByMerchant = await _db.FindAsync(a => a.AccountId == merchantId);
            //return productsByMerchant.ToList(); 
        }

        public bool InsertComment(ProductComment product)
        {
            try
            {

                _dbProductComment.InsertOne(product);

                return true;

            

            }
            catch
            {
                return false;
            }
        }

        public bool InsertProductAction(List<ProductAction> productActions)
        {
            try
            {
                _dbProductAction.InsertMany(productActions);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProductAction(ProductAction product)
        {
            try
            {
                var userAction = _dbProductAction.Find(p => p.UserId == product.UserId).ToList();
                var productByEnduser = userAction.FirstOrDefault(s => s.ProductId == product.ProductId);

                var deleteFilter = Builders<ProductAction>.Filter.Eq("_id", productByEnduser._id);
                _dbProductAction.DeleteOne(deleteFilter);
               

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Product> GetProductsBought(string userId)
        {
            var prActionollection = _dbProductAction;
            var productCollection = _db;
            var Info = (from product in productCollection.AsQueryable()
                        join prAction in prActionollection.AsQueryable() on product._id equals prAction.ProductId
                        where prAction.UserId == userId && prAction.Status == ProductActionConstant.PRODUCT_BOUGHT
                        select new Product()
                        {
                            _id = product._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            IdItemType = product.IdItemType,                 
                            AccountId = product.AccountId
                        });

            return Info.ToList();         
        }

        public List<Product> GetProductsFavorite(string userId)
        {
            var prActionollection = _dbProductAction;
            var productCollection = _db;
            var Info = (from product in productCollection.AsQueryable()
                        join prAction in prActionollection.AsQueryable() on product._id equals prAction.ProductId
                        where prAction.UserId == userId && prAction.Status == ProductActionConstant.PRODUCT_FAVORITE
                        select new Product()
                        {
                            _id = product._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            IdItemType = product.IdItemType,
                            AccountId = product.AccountId
                        });

            return Info.ToList();
        }
    }
}
