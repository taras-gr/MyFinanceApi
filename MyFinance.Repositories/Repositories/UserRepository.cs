namespace MyFinance.Repositories.Repositories
{
    //public class UserRepository : IUserRepository
    //{
    //    private readonly UserDbContext _context;

    //    public UserRepository(IOptions<MongoDbSettings> setiings)
    //    {
    //        _context = new UserDbContext(setiings);
    //    }

    //    public async Task AddUser(User user)
    //    {
    //        await _context.Users.InsertOneAsync(user);
    //    }

    //    public Task<bool> DeleteUser(string userId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<User> GetUserByEmail(string email)
    //    {
    //        var filter = new BsonDocument("Email", email);
    //        var userFromRepo = await _context.Users.Find(filter).FirstOrDefaultAsync();

    //        return userFromRepo;
    //    }

    //    public async Task<User> GetUserById(ObjectId userId)
    //    {
    //        var userFromRepo = await _context.Users.Find(user => user.Id == userId).FirstOrDefaultAsync();

    //        return userFromRepo;
    //    }

    //    public Task<User> GetUserByName(string userName)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<List<User>> GetUsers()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> UpdateUser(string userId, User user)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
