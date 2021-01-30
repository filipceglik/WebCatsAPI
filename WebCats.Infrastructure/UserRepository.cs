using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WebCats.Model;

namespace WebCats.Infrastructure
{
    public class UserRepository
    {
        private readonly DatabaseContext _databaseContext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public async Task<User> GetUser(string username) => await _databaseContext
            .GetCollection<User>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.UserName == username);

        public async Task Create(User user)
        {
            await _databaseContext
                .GetCollection<User>()
                .InsertOneAsync(user);
        }
    }
}