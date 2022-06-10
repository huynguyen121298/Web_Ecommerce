using DataAndServices.CommonModel;
using DataAndServices.Data;
using DataAndServices.DataModel;
using Model.Common;
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
        private readonly IMongoCollection<ProductRecommend> _dbProductRecommend;

        public ProductClientService(DataContext db)
        {
            _db = db.GetProductClientCollection();
            _dbItem = db.GetItemCollection();
            _dbDis = db.GetDiscountProductCollection();
            _dbAcc = db.GetAccountCollection();
            _dbProductComment = db.GetProductCommentCollection();
            _dbProductAction = db.GetProductActionCollection();
            _dbProductRecommend = db.GetProductRecommendCollection();
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
                            Rating = product.Rating,
                            AccountId = product.AccountId,
                            Price_Dis = dis.Price_Dis,
                            Start = dis.Start,
                            End = dis.End
                        }).Where(s => s.Name.ToLower().Contains(name.ToLower())).ToList();
            return Info;
        }

        public List<Account> GetMerchantByName(string merchantName)
        {
            var merchantByName = _dbAcc.Find(s => s.MerchantName.ToLower().Contains(merchantName.ToLower())).ToList();
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
                            Rating = product.Rating,
                            AccountId = product.AccountId,                         
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

        public async Task<int> GetSoLuong(string itemId)
        {
            var temp = await _dbItem.Find(s => s._id == itemId).FirstOrDefaultAsync();
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
                            AccountId = product.AccountId,
                            Rating = product.Rating
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
                //case 1: favorite
                var checkTypeAction = productActions.FirstOrDefault();
                if (checkTypeAction.Status == ProductActionConstant.PRODUCT_FAVORITE)
                {
                    var proAction = _dbProductAction.Find(a => a.UserId == checkTypeAction.UserId
                                                           && a.Status == ProductActionConstant.PRODUCT_FAVORITE
                                                           && a.ProductId == checkTypeAction.ProductId).FirstOrDefault();

                    if (proAction == null)
                    {
                        _dbProductAction.InsertOne(checkTypeAction);
                    }
                    return true;
                }

                //case 2: history
                var actions = _dbProductAction.Find(a => a.UserId == productActions.FirstOrDefault().UserId).ToList();
                var newProductActions = new List<ProductAction>();
                foreach(var action in productActions)
                {
                    var userAction = actions.Where(a => a.ProductId == action.ProductId).FirstOrDefault();
                    if (userAction ==null)
                    {
                        newProductActions.Add(action);
                    }
                }
                _dbProductAction.InsertMany(newProductActions);
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
                var userAction = _dbProductAction.Find(p => p.UserId == product.UserId
                                                       && p.ProductId == product.ProductId
                                                       && p.Status==product.Status).FirstOrDefault();

                var deleteFilter = Builders<ProductAction>.Filter.Eq("_id", userAction._id);
                _dbProductAction.DeleteOne(deleteFilter);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Dis_Product> GetProductsBought(string userId)
        {
            var discountCollection = _dbDis;
            var productCollection = _db;
            var prAction = _dbProductAction.Find(a => a.UserId == userId && a.Status == ProductActionConstant.PRODUCT_BOUGHT).ToList();

            var products = new List<Dis_Product>();
            var prod = (from dis in discountCollection.AsQueryable()
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
                            End = dis.End,
                            AccountId = product.AccountId,
                            Rating = product.Rating
                        }).ToList();

            foreach (var product in prAction)
            {
                var pro = prod.Find(p => p._id == product.ProductId);
                var checkPro = products.Contains(pro);
                if (!checkPro)
                    products.Add(pro);
            }

            return products;
        }

        public List<Product> GetProductsFavorite(string userId)
        {
            var prAction = _dbProductAction.Find(a => a.UserId == userId && a.Status == ProductActionConstant.PRODUCT_FAVORITE).ToList();

            var products = new List<Product>();
            var prod = _db.Find(s => true).ToList();
            foreach (var product in prAction)
            {
                var pro = prod.Find(p => p._id == product.ProductId);
                products.Add(pro);
            }

            return products;
        }

        public async Task<bool> InsertProductRecommend(ProductRecommend productRecommend)
        {
            try
            {
                var dbProductRecommend = await _dbProductRecommend.Find(pr => pr.ProductId == productRecommend.ProductId).FirstOrDefaultAsync();
                if (dbProductRecommend == null)
                {
                    var product = await _db.Find(p => p._id == productRecommend.ProductId).FirstOrDefaultAsync();
                    var dbproductRecommend = new ProductRecommend
                    {
                        ItemTypeId = product.IdItemType,
                        Frequency = 1,
                        ProductId = productRecommend.ProductId,
                        UserId = productRecommend.UserId,
                    };
                    await _dbProductRecommend.InsertOneAsync(dbproductRecommend);
                    return true;
                }

                var eqfilter = Builders<ProductRecommend>.Filter.Where(s => s.ProductId == productRecommend.ProductId);
                var newFrequency = dbProductRecommend.Frequency + 1;

                var update = Builders<ProductRecommend>.Update.Set(s => s.Frequency,newFrequency );

                var options = new UpdateOptions { IsUpsert = true };

                await _dbProductRecommend.UpdateOneAsync(eqfilter, update, options);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Product> GetProductRecommend()
        {
            var recommends = _dbProductRecommend.Find(p => true).ToList();
            var orderByDescendingResult = (from product in recommends
                                           orderby product.Frequency descending
                                           select product);

            orderByDescendingResult.Take(10);

            var products = _db.Find(p => true).ToList();
            //var productRecomends = new List<Product>();

            var prod = (from recommend in orderByDescendingResult.AsQueryable()
                        join product in products.AsQueryable() on recommend.ProductId equals product._id
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
                            AccountId = product.AccountId,
                            Rating = product.Rating
                        }).ToList();

            //foreach (var productId in orderByDescendingResult.Select(p=>p.ProductId))
            //{
            //    var productRecomend = products.Find(p => p._id == productId);
            //    if
            //}

            //return orderByDescendingResult;
            return prod;
        }

        public async Task<bool> UpdateRating(Product productRating)
        {
            var product = await _db.Find(p => p._id == productRating._id).FirstOrDefaultAsync();
            if (product == null)
                return false;
            var newRating = (product.Rating + productRating.Rating) / 2;
            var eqfilter = Builders<Product>.Filter.Where(s => s._id == productRating._id);

            var update = Builders<Product>.Update.Set(s => s.Rating, newRating);

            var options = new UpdateOptions { IsUpsert = true };

            await _db.UpdateOneAsync(eqfilter, update, options);

            return true;
        }
    }
}