using MongoDB.Driver;

namespace H_Shop.NetCore.Data
{
    public class DataContext
    {
        private MongoClient mongoClient { get; }

        private IMongoDatabase Database { get; }

        public DataContext(string connectionString, string dbName)
        {
            mongoClient = new MongoClient(connectionString);
            Database = mongoClient.GetDatabase(dbName);
        }  
    }
}
