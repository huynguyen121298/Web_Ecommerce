using DataAndServices.CommonModel;
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
        private readonly IMongoCollection<Product_Admin> _db;
        private readonly IMongoCollection<Discount_Product> _dbDis;
        private readonly IMongoCollection<Item> _dbItem;
        private readonly IMongoCollection<Item_type> _dbItemtype;
        private readonly DataContext db = new DataContext("mongodb://localhost:27017", "OnlineShop");

        public ProductService(DataContext db)
        {
            _db = db.GetProductAdminCollection();
            _dbDis = db.GetDiscountProductCollection();
            _dbItem = db.GetItemCollection();
            _dbItemtype = db.GetItem_TypeCollection();
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
                Product_Admin products = new Product_Admin();
                products.Id_Item = product_Item_Type.Id_Item;
                products.Name = product_Item_Type.Name;
                products.Photo = product_Item_Type.Photo;
                products.Photo2 = product_Item_Type.Photo2;
                products.Photo3 = product_Item_Type.Photo3;
                products.Price = product_Item_Type.Price;
                products.Details = product_Item_Type.Details;              
                _db.InsertOne(products);

                Item item = new Item();
                item.Quantity = product_Item_Type.Quantity;
                item._id = products._id;
                _dbItem.InsertOne(item);

                Discount_Product dis = new Discount_Product();
                dis._id = item._id;
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
                
                var deleteFilter = Builders<Product_Admin>.Filter.Eq("_id", id);
                var deleteFilter2 = Builders<Item>.Filter.Eq("_id", id);
                var deleteFilter3 = Builders<Discount_Product>.Filter.Eq("_id", id);

                _db.DeleteOne(deleteFilter);
               
                _dbItem.DeleteOne(id);

                _dbDis.DeleteOne(id);
            
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Product_Item_Type>> GetAllProductItem()
        {
            var itemCollection =  db.GetItemCollection();
            var productCollection = db.GetProductClientCollection();
            var infoProduct = from item in itemCollection.AsQueryable()
                              join product in productCollection.AsQueryable() on item._id equals product._id

                              select new Product_Item_Type()
                              {
                                  _id = product._id,
                                  Name = product.Name,
                                  Price = product.Price,
                                  Details = product.Details,
                                  Photo = product.Photo,
                                  Photo2 = product.Photo2,
                                  Photo3 = product.Photo3,
                                  Id_Item = product.Id_Item,
                                  Quantity = item.Quantity

                              };
            return await infoProduct.ToListAsync();
        }

        public List<List<Dis_Product>> GetAllProductItem_Type()
        {
            List<Item_type> item_Types = new List<Item_type>();
            List<List<Dis_Product>> productsByType = new List<List<Dis_Product>>();
            item_Types = _dbItemtype.Find(s=>true).ToList();
            foreach (Item_type item in item_Types)
            {

                List<Dis_Product> products = GetProductById_Item(item.Id_Item);
                productsByType.Add(products);
            }
            return productsByType.ToList();
        }

        public async Task<List<Product_Admin>> GetAllProducts()
        {
            return await _db.Find(s => true).ToListAsync();
        }

        public List<Dis_Product> GetAllProduct_Discount()
        {
            var discountCollection = db.GetDiscountProductCollection();
            var productCollection = db.GetProductClientCollection();

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
                            Id_Item = product.Id_Item,
                            Content = dis.Content,
                            Price_Dis = dis.Price_Dis,
                            Start = dis.Start,
                            End = dis.End
                        });

            return  Info.ToList();  
        }

        public async Task<Product_Admin> GetProductById(string id)
        {

            return await _db.Find(s => s._id == id).FirstOrDefaultAsync();
        }

        public List<Dis_Product> GetProductById_Item(int id)
        {
            var discountCollection = db.GetDiscountProductCollection();
            var productCollection = db.GetProductClientCollection();
            var Info = (from dis in discountCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on dis._id equals product._id
                        where product.Id_Item==id
                        select new Dis_Product()
                        {
                            _id = dis._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            Id_Item = product.Id_Item,
                            Content = dis.Content,
                            Price_Dis = dis.Price_Dis,
                            Start = dis.Start,
                            End = dis.End
                        });

            return Info.ToList();
        }

        public  Product_Item_Type GetProductItemById(string id)
        {
            var itemCollection = db.GetItemCollection();
            var productCollection = db.GetProductClientCollection();
            var Info =  (from item in itemCollection.AsQueryable()
                       join product in productCollection.AsQueryable() on item._id equals product._id
                       where  item._id == id
                       select new Product_Item_Type()
                       {
                           _id = item._id,
                           Name = product.Name,
                           Price = product.Price,
                           Details = product.Details,
                           Photo = product.Photo,
                           Photo2 = product.Photo2,
                           Photo3 = product.Photo3,
                           Id_Item = product.Id_Item,
                           Quantity = item.Quantity

                       }).FirstOrDefault();
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
            var itemCollection = db.GetItemCollection();
            var productCollection = db.GetProductClientCollection();
            var Info = (from item in itemCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on item._id equals product._id
                        select new Product_Item_Type()
                        {
                            _id = product._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            Id_Item = product.Id_Item,
                            Quantity = item.Quantity
                        }).FirstOrDefault();
           
            return  Info;
        }
    

        public Product_Item_Type GetProductItemById_admin(string id)
        {
            var itemCollection = db.GetItemCollection();
            var productCollection = db.GetProductClientCollection();
            var Info = (from item in itemCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on item._id equals product._id
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
                            Id_Item = product.Id_Item,
                            Quantity = item.Quantity
                        });

            return Info.FirstOrDefault();
            
        }

        public List<Product_Item_Type> GetProductItemById_client(int id)
        {
            var itemTypeCollection = db.GetItem_TypeCollection();
            var productCollection = db.GetProductClientCollection();
            var Info = (from item in itemTypeCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on item.Id_Item equals product.Id_Item
                        where  item.Id_Item == id
                        select new Product_Item_Type()
                        {
                            _id = item._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            Id_Item = product.Id_Item,
                            Type_Product = item.Type_Product
                        }).ToList();

            foreach (var item in Info)
            {
                if (GetPriceDiscountByIdList(item._id) != 0)
                {
                    item.Price = Convert.ToInt32(GetPriceDiscountById(item._id));
                }
            }
            return  Info.ToList();
        }

        public List<Product_Item_Type> GetProductItemByPageList()
        {
            var itemCollection = db.GetItemCollection();
            var productCollection = db.GetProductClientCollection();
            var Info = (from item in itemCollection.AsQueryable()
                        join product in productCollection.AsQueryable() on item._id equals product._id
                        orderby item._id
                        select new Product_Item_Type()
                        {
                            _id = item._id,
                            Name = product.Name,
                            Price = product.Price,
                            Details = product.Details,
                            Photo = product.Photo,
                            Photo2 = product.Photo2,
                            Photo3 = product.Photo3,
                            Id_Item = product.Id_Item,
                            Quantity = item.Quantity
                        });

            return  Info.ToList();
        }

        public Dis_Product GetProduct_DiscountById(string id)
        {
            var discountProCollection = db.GetDiscountProductCollection();
            var productCollection = db.GetProductClientCollection();
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
                                           Id_Item = product.Id_Item,
                                           Content = dis.Content,
                                           Price_Dis = dis.Price_Dis,
                                           Start = dis.Start,
                                           End = dis.End
                                       };
            return  infoProduct_discount.FirstOrDefault();
        }

        public bool InsertProduct_Discount(Discount_Product custom)
        {

            Discount_Product prodItem =  _dbDis.Find(p => p._id == custom._id).FirstOrDefault();
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
                var eqfilter = Builders<Product_Admin>.Filter.Where(s => s._id == custom._id);
                var update = Builders<Product_Admin>.Update.Set(s => s.Name, custom.Name)
                    .Set(s => s.Photo, custom.Photo)
                    .Set(s => s.Photo2, custom.Photo2)
                    .Set(s => s.Photo3, custom.Photo3)
                    .Set(s => s.Details, custom.Details)
                    .Set(s => s.Price, custom.Price)
                    .Set(s => s._id, custom._id)
                    .Set(s => s.Id_Item, custom.Id_Item);

                var options = new UpdateOptions { IsUpsert = true };
                _db.UpdateOneAsync(eqfilter, update, options).ConfigureAwait(false);

                var eqfilter2 = Builders<Item>.Filter.Where(s => s._id == custom._id);
                var update2 = Builders<Item>.Update.Set(s => s._id, custom._id)
                    .Set(s => s.Quantity, custom.Quantity);                   

                var options2 = new UpdateOptions { IsUpsert = true };
                _dbItem.UpdateOneAsync(eqfilter2, update2, options2).ConfigureAwait(false);

                return true;
            }   
            catch (Exception)
            {
                return false;
            }
        }

       
    }
}


