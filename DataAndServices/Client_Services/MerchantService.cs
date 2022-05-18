using DataAndServices.CommonModel;
using DataAndServices.Data;
using DataAndServices.DataModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DataAndServices.Client_Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IMongoCollection<Product> _db;
        private readonly IMongoCollection<Discount_Product> _dbDis;
        private readonly IMongoCollection<Item_type> _dbItemtype;
        private readonly IMongoCollection<ProductComment> _dbProductComment;
        public MerchantService(DataContext db)
        {
            _db = db.GetProductClientCollection();
            _dbDis = db.GetDiscountProductCollection();
            _dbItemtype = db.GetItem_TypeCollection();
            _dbProductComment = db.GetProductCommentCollection();

        }
        public List<Dis_Product> GetAllProductByName(string name, string merchantId)
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
                            End = dis.End
                        }).Where(s => s.Name.StartsWith(name)).ToList();
            return Info;
        }

        public List<Dis_Product> GetAllProductByPrice(int giaMin, int giaMax, string merchantId)
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

                    bool dis_Product7 = (item.Price_Dis <= giaMax && item.Price_Dis >= giaMin);
                    if (dis_Product7 == true)
                    {
                        dis_Product5.Add(item);
                    }

                }
                else
                {
                    bool dis_Product8 = (item.Price <= giaMax && item.Price >= giaMin);
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

        public List<Product_Item_Type> GetProductItemById_client(string id, string merchantId)
        {
            var itemTypeCollection = _dbItemtype.Find(s=>s.Status == "Đã kích hoạt").ToList();
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
                            AccountId = product.AccountId
                        }).ToList();
            var products = Info.Where(p => p.AccountId == merchantId);
            foreach (var item in products)
            {
                var productComment = _dbProductComment.Find(cmt => cmt.ProductId == item._id).ToList();
                item.Comments = productComment;
                if (GetPriceDiscountByIdList(item._id) != 0)
                {
                    item.Price = Convert.ToInt32(GetPriceDiscountById(item._id));
                }
            }
            return products.ToList();
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
    }
}
