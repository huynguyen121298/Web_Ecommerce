﻿using DataAndServices.DataModel;
using MongoDB.Driver;

namespace DataAndServices.Data
{
    public class DataContext
    {
        private MongoClient mongoClient { get; }
        private IMongoDatabase Database { get; }
        //public DataContext(string connectionString, string dbName)
        //{
        //    mongoClient = new MongoClient("mongodb://localhost:27017");
        //    Database = mongoClient.GetDatabase("OnlineShop");
        //}
        public DataContext(string connectionString, string dbName)
        {
            mongoClient = new MongoClient("mongodb+srv://huyadmin2:Huyhuy123@cluster0.plas1.mongodb.net/OnlineShop?retryWrites=true&w=majority");
            Database = mongoClient.GetDatabase("OnlineShop");
        }

        public IMongoCollection<Account> GetAccountCollection()
        {
            return Database.GetCollection<Account>("Account");
        }
        public IMongoCollection<Product> GetProductAdminCollection()
        {
            return Database.GetCollection<Product>("Product");
        }

        public IMongoCollection<ProductAction> GetProductActionCollection()
        {
            return Database.GetCollection<ProductAction>("ProductAction");
        }
        public IMongoCollection<Product> GetProductClientCollection()
        {
            return Database.GetCollection<Product>("Product");
        }
        public IMongoCollection<Checkout_Customer> GetCheckout_CustomerCollection()
        {
            return Database.GetCollection<Checkout_Customer>("Checkout_Customer");
        }

        public IMongoCollection<ProductComment> GetProductCommentCollection()
        {
            return Database.GetCollection<ProductComment>("ProductComment");
        }

        public IMongoCollection<ProductRecommend> GetProductRecommendCollection()
        {
            return Database.GetCollection<ProductRecommend>("ProductRecommend");
        }

        public IMongoCollection<MerchantNotification> GetMerchantNotificationCollection()
        {
            return Database.GetCollection<MerchantNotification>("MerchantNotification");
        }
        public IMongoCollection<Checkout_Oder> GetCheckout_OderCollection()
        {
            return Database.GetCollection<Checkout_Oder>("Checkout_Oder");
        }
        public IMongoCollection<User_Acc> GetUser_AccCollection()
        {
            return Database.GetCollection<User_Acc>("Users_Acc");
        }
        public IMongoCollection<User_Acc_Client> GetUser_Acc_ClientCollection()
        {
            return Database.GetCollection<User_Acc_Client>("Users_Acc");
        }
        public IMongoCollection<Role> GetRoleCollection()
        {
            return Database.GetCollection<Role>("Role");
        }
        public IMongoCollection<Item> GetItemCollection()
        {
            return Database.GetCollection<Item>("Item");
        }
        public IMongoCollection<Item_Client> GetItem_ClientCollection()
        {
            return Database.GetCollection<Item_Client>("Item");
        }
        public IMongoCollection<Item_type> GetItem_TypeCollection()
        {
            return Database.GetCollection<Item_type>("Item_Type");
        }
        public IMongoCollection<CodeDiscount> GetCodeDiscountCollection()
        {
            return Database.GetCollection<CodeDiscount>("CodeDiscount");
        }
        public IMongoCollection<Feedback> GetFeedbackCollection()
        {
            return Database.GetCollection<Feedback>("Feedback");
        }
        public IMongoCollection<Discount_Product> GetDiscountProductCollection()
        {
            return Database.GetCollection<Discount_Product>("Discount_Product");
        }
        public IMongoCollection<CheckoutCustomerOrder> GetCheckoutCustomerOrderCollection()
        {
            return Database.GetCollection<CheckoutCustomerOrder>("CheckoutCustomerOrder");
        }
    }
}
