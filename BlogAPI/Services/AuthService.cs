using BlogAPI.Models;
using MongoDB.Driver;

namespace BlogAPI.Services;

public class AuthService: IAuthService
{
    private readonly IMongoCollection<User> _users;
    
    public AuthService(IMongoDBDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _users = database.GetCollection<User>(settings.UserCollectionName);
    }

    public User UserRegister(User user)
    {
        _users.InsertOne(user);
        return user;
    }

    public User GetUserById(string id)
    {
        return _users.Find(u => u.Id == id).FirstOrDefault();
    }

    public User GetUserByUserName(string username)
    {
        return _users.Find(u => u.Username == username).FirstOrDefault();
    }

    public User GetUserByEmail(string email)
    {
        return _users.Find(u => u.Email == email).FirstOrDefault();
    }
}