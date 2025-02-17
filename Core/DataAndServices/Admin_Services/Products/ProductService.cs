﻿using DataAndServices.CommonModel;
using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DataAndServices.Admin_Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _db;
        private readonly IMongoCollection<Discount_Product> _dbDis;
        private readonly IMongoCollection<Item> _dbItem;
        private readonly IMongoCollection<Item_type> _dbItemtype;
        private readonly IMongoCollection<ProductComment> _dbProductComment;

        //private readonly DataContext db = new DataContext("mongodb://localhost:27017", "OnlineShop");

        public ProductService(DataContext db)
        {
            _db = db.GetProductAdminCollection();
            _dbDis = db.GetDiscountProductCollection();
            _dbItem = db.GetItemCollection();
            _dbItemtype = db.GetItem_TypeCollection();
            _dbProductComment = db.GetProductCommentCollection();
        }

        public double GetPriceDiscountByIdList(string id)
        {
            DateTime dateTime = DateTime.Today;
            var item_discount = _dbDis.Find(t => t._id == id && t.End.Value >= dateTime).ToList();
            foreach (var item in item_discount)
            {
                if (item != null && item.Price_Dis != null)
                    return Convert.ToDouble(item.Price_Dis);
            }

            return 0;
        }

        public int CreateProduct(Product_Item_Type product_Item_Type)
        {
            try
            {
                Product products = new Product();

                products.Name = product_Item_Type.Name;
                products.Photo = product_Item_Type.Photo;
                products.Photo2 = product_Item_Type.Photo2;
                products.Photo3 = product_Item_Type.Photo3;
                products.Price = product_Item_Type.Price;
                products.Details = product_Item_Type.Details;
                products.AccountId = product_Item_Type.AccountId;
                products.Rating = 5;

                var itemType = _dbItemtype.Find(s => s.Type_Product == product_Item_Type.Type_Product).FirstOrDefault();

                products.IdItemType = itemType._id;
                _db.InsertOne(products);

                var items = new List<Item>();
                foreach (var i in product_Item_Type.Items)
                {
                    var item = new Item();
                    item.Quantity = product_Item_Type.QuantityBuy;
                    item.ProductId = products._id;
                    item.Size = i.Size;
                    item.Color = i.Color;
                    items.Add(item);
                }

                _dbItem.InsertMany(items);

                Discount_Product dis = new Discount_Product();
                dis._id = products._id;
                _dbDis.InsertOne(dis);

                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public bool DeleteAccount(string id)
        {
            try
            {
                var deleteFilter = Builders<Product>.Filter.Eq("_id", id);
                var deleteFilter2 = Builders<Item>.Filter.Eq("ProductId", id);
                var deleteFilter3 = Builders<Discount_Product>.Filter.Eq("_id", id);
                var deleteFilter4 = Builders<ProductComment>.Filter.Eq("ProductId", id);

                _db.DeleteOne(deleteFilter);

                _dbItem.DeleteMany(deleteFilter2);

                _dbDis.DeleteOne(deleteFilter3);
                _dbProductComment.DeleteOne(deleteFilter4);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Product_Item_Type>> GetAllProductItem(string userLogin)
        {
            var itemCollection = _dbItem;
            var items = _dbItem.Find(s => true).ToList();
            var productCollection = _db;
            var infoProduct = (from item in itemCollection.AsQueryable()
                              join product in productCollection.AsQueryable() on item.ProductId equals product._id
                              where product.AccountId == userLogin

                              select new Product_Item_Type()
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
                                  Rating = product.Rating,
                                  Items = items.Where(s=>s.ProductId==product._id).ToList(),
                              }).ToListAsync();
            
            return await infoProduct;
        }

        public async Task<List<Product_Item_Type>> GetAllProductItemByEndUser()
        {
            var itemCollection = _dbItem;
            var items = _dbItem.Find(s => true).ToList();
            var productCollection = _db;
            var infoProduct = from item in itemCollection.AsQueryable()
                              join product in productCollection.AsQueryable() on item.ProductId equals product._id

                              select new Product_Item_Type()
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
                                  Items = items.Where(s => s.ProductId == product._id).ToList(),
                                  Rating = product.Rating,
                              };
            foreach (var item in infoProduct)
            {
                var productComment = _dbProductComment.Find(cmt => cmt.ProductId == item._id).ToList();

                item.Comments = productComment;
            }
            return await infoProduct.ToListAsync();
        }

        public List<List<Dis_Product>> GetAllProductItem_Type(string userLogin)
        {
            List<Item_type> item_Types = new List<Item_type>();
            List<List<Dis_Product>> productsByType = new List<List<Dis_Product>>();
            item_Types = _dbItemtype.Find(s => true).ToList();
            foreach (Item_type item in item_Types)
            {
                List<Dis_Product> products = GetProductById_Item(item._id);
                var productsByMerchant = products.Where(p => p.AccountId == userLogin).ToList();
                productsByType.Add(productsByMerchant);
            }
            return productsByType.ToList();
        }

        public List<List<Dis_Product>> GetAllProductItem_TypeByEndUser()
        {
            List<Item_type> item_Types = new List<Item_type>();
            List<List<Dis_Product>> productsByType = new List<List<Dis_Product>>();
            item_Types = _dbItemtype.Find(s => true).ToList();
            foreach (Item_type item in item_Types)
            {
                List<Dis_Product> products = GetProductById_Item(item._id);

                productsByType.Add(products);
            }
            return productsByType.ToList();
        }

        public async Task<List<Product>> GetAllProducts(string userLogin)
        {
            return await _db.Find(s => s.AccountId == userLogin).ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsByEndUser()
        {
            return await _db.FindSync(s => true).ToListAsync();
        }

        public List<Dis_Product> GetAllProduct_Discount(string userLogin)
        {
            var discountCollection = _dbDis;
            var productCollection = _db;

            var Info = (from dis in discountCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on dis._id equals product._id
                        where product.AccountId == userLogin
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
        }

        public List<Dis_Product> GetAllProduct_Discount()
        {
            var discountCollection = _dbDis;
            var productCollection = _db;

            var Info = (from dis in discountCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on dis._id equals product._id
                        orderby product._id descending
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
                            Rating = product.Rating,
                                  
                        });

            return Info.ToList();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _db.Find(s => s._id == id).FirstOrDefaultAsync();
        }

        public List<Dis_Product> GetProductById_Item(string id,string userLogin)
        {
            var typeProduct = _dbItemtype.Find(s => s._id == id).FirstOrDefault();
            var discountCollection = _dbDis;
            var productCollection = _db;
            var itemTypeCollection = _dbItemtype;
            var Info = (from dis in discountCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on dis._id equals product._id
                        where product.IdItemType == id && product.AccountId == userLogin
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
                            Type_Product = typeProduct.Type_Product,
                            Price_Dis = dis.Price_Dis,
                            Start = dis.Start,
                            End = dis.End,
                            AccountId = product.AccountId,
                            Rating = product.Rating
                        });

            return Info.ToList();
        }

        public List<Dis_Product> GetProductById_Item(string id)
        {
            var typeProduct = _dbItemtype.Find(s => s._id == id).FirstOrDefault();
            var discountCollection = _dbDis;
            var productCollection = _db;
            var itemTypeCollection = _dbItemtype;
            var Info = (from dis in discountCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on dis._id equals product._id
                        where product.IdItemType == id
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
                            Type_Product = typeProduct.Type_Product,
                            Price_Dis = dis.Price_Dis,
                            Start = dis.Start,
                            End = dis.End,
                            AccountId = product.AccountId,
                            Rating = product.Rating
                        });

            return Info.ToList();
        }

        public Product_Item_Type GetProductItemById(string id)
        {
            var itemCollection = _dbItem;
            var product = _db.Find(p=>p._id == id).FirstOrDefault();
            var Info =
                        new Product_Item_Type()
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
                        };
            var itemPros = _dbItem.Find(s => s.ProductId == id).ToList();
            Info.Items = itemPros;

            var productComment = _dbProductComment.Find(cmt => cmt.ProductId == Info._id).ToList();
            Info.Comments = productComment;

            if (GetPriceDiscountById(Info._id) != 0)
            {
                Info.Price = Convert.ToInt32(GetPriceDiscountById(Info._id));
            }
            return Info;
        }

        private double GetPriceDiscountById(string id)
        {
            DateTime dateTime = DateTime.Today;
            var item_discount = _dbDis.Find(t => t._id == id && t.End.Value >= dateTime).FirstOrDefault();

            if (item_discount != null && item_discount.Price_Dis != null)
            {
                return Convert.ToDouble(item_discount.Price_Dis);
            }
            return 0;
        }

        public Product_Item_Type GetProductItemById2(string id)
        {
            var itemCollection = _dbItem;
            var product = _db.Find(p => p._id == id).FirstOrDefault();
            var Info =
                        new Product_Item_Type()
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
                        };
            var itemPros = _dbItem.Find(s => s.ProductId == id).ToList();
            Info.Items = itemPros;

            var productComment = _dbProductComment.Find(cmt => cmt.ProductId == Info._id).ToList();
            Info.Comments = productComment;

            return Info;
        }

        public Product_Item_Type GetProductItemById_admin(string id)
        {
            var itemCollection = _dbItem;
            var product = _db.Find(p => p._id == id).FirstOrDefault();
            var Info =
                        new Product_Item_Type()
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
                        };
            var itemPros = _dbItem.Find(s => s.ProductId == id).ToList();
            Info.Items = itemPros;

            var productComment = _dbProductComment.Find(cmt => cmt.ProductId == id).ToList();
            Info.Comments = productComment;
            return Info;
        }

        public List<Product_Item_Type> GetProductItemById_client(string id)
        {
            var itemTypeCollection = _dbItemtype.Find(s => s.Status == "Đã kích hoạt").ToList();
            var productCollection = _db;
            var Info = (from item in itemTypeCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on item._id equals product.IdItemType
                        where item._id == id
                        select new Product_Item_Type()
                        {
                            _id = item._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            IdItemType = product.IdItemType,
                            Type_Product = item.Type_Product,
                            AccountId = product.AccountId,
                            Rating = product.Rating
                        }).ToList();

            foreach (var item in Info)
            {
                var productComment = _dbProductComment.Find(cmt => cmt.ProductId == item._id).ToList();
                item.Comments = productComment;
                if (GetPriceDiscountByIdList(item._id) != 0)
                {
                    item.Price = Convert.ToInt32(GetPriceDiscountById(item._id));
                }
            }
            return Info.ToList();
        }

        public List<Product_Item_Type> GetProductItemByPageList()
        {
            var productCollection = _db;
            var Info = (from product in productCollection.AsQueryable()
                        orderby product._id
                        select new Product_Item_Type()
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
                        });

            return Info.ToList();
        }

        public Dis_Product GetProduct_DiscountById(string id)
        {
            var discountProCollection = _dbDis;
            var productCollection = _db;
            var infoProduct_discount = from dis in discountProCollection.AsQueryable()
                                       join product in productCollection.AsQueryable() on dis._id equals product._id
                                       where dis._id == id
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
                                       };
            return infoProduct_discount.FirstOrDefault();
        }

        public bool InsertProduct_Discount(Discount_Product custom)
        {
            Discount_Product prodItem = _dbDis.Find(p => p._id == custom._id).FirstOrDefault();
            if (prodItem != null)
            {
                var eqfilter = Builders<Discount_Product>.Filter.Where(s => s._id == custom._id);
                var update = Builders<Discount_Product>.Update.Set(s => s._id, custom._id)
                    .Set(s => s.Price_Dis, custom.Price_Dis)
                    .Set(s => s.Content, custom.Content)
                    .Set(s => s.Start, custom.Start)
                    .Set(s => s.End, custom.End);

                var options = new UpdateOptions { IsUpsert = true };
                _dbDis.UpdateOneAsync(eqfilter, update, options).ConfigureAwait(false);

                return true;
            }
            else
            {
                _dbDis.InsertOne(custom);
            }

            return false;
        }

        public bool UpdateProduct(Product_Item_Type custom)
        {
            try
            {
                var itemType = _db.Find(s => s._id == custom._id).FirstOrDefault();
                var newItemType = _dbItemtype.Find(s => s.Type_Product == custom.Type_Product).FirstOrDefault();

                var eqfilter = Builders<Product>.Filter.Where(s => s._id == custom._id);
                var update = Builders<Product>.Update.Set(s => s.Name, custom.Name)
                    .Set(s => s.Photo, custom.Photo)
                    .Set(s => s.Photo2, custom.Photo2)
                    .Set(s => s.Photo3, custom.Photo3)
                    .Set(s => s.Details, custom.Details)
                    .Set(s => s.Price, custom.Price)
                    .Set(s => s._id, custom._id)
                    .Set(s => s.IdItemType, newItemType._id);

                var options = new UpdateOptions { IsUpsert = true };
                _db.UpdateOneAsync(eqfilter, update, options).ConfigureAwait(false);

                //if(itemType.IdItemType != newItemType._id)
                //{
                //    var eqfilter2 = Builders<Item>.Filter.Where(s => s.ProductId == custom._id);

                //    _dbItem.DeleteMany(eqfilter2);
                //}

                //var items = new List<Item>();
                //foreach (var i in custom.Items)
                //{
                //    var item = new Item();
                //    item.Quantity = i.Quantity;
                //    item.ProductId = custom._id;
                //    item.Size = i.Size;
                //    item.Color = i.Color;
                //    items.Add(item);
                //}

                //_dbItem.InsertMany(items);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}