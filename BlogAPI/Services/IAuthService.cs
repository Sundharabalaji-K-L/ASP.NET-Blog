using BlogAPI.Models;

namespace BlogAPI.Services;

public interface IAuthService
{
    User UserRegister(User user);
    User GetUserById(string id);

    User GetUserByUserName(string username);
    User GetUserByEmail(string email);
}