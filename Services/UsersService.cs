using BudgetApp.Models.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BudgetApp.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UsersService (IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient (databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase (databaseSettings.Value.DatabaseName);
            _usersCollection = mongoDatabase.GetCollection<User>(databaseSettings.Value.UsersCollectionName);
        }

        public async Task<List<User>> GetAsync() => 
            await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string id) => 
            await _usersCollection.Find(x => x._id == id).FirstOrDefaultAsync();

        public async Task<User?> GetEmailAsync(string email) =>
            await _usersCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) => 
            await _usersCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) => 
            await _usersCollection.ReplaceOneAsync(x => x._id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _usersCollection.DeleteOneAsync(x => x._id == id);
    }
}
