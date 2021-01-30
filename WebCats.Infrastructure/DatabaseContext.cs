using MongoDB.Driver;

namespace WebCats.Infrastructure
{
    public class DatabaseContext
    {
        private readonly IMongoDatabase _database;

        public DatabaseContext(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("WebCatsDB");
        }

        public IMongoCollection<TModel> GetCollection<TModel>() => _database.GetCollection<TModel>(typeof(TModel).Name);
    }
}