using BCrypt.Net;
using BlogAPI.Models;

namespace BlogAPI.Services;

public class PasswordHelper
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
        
    }

    public static bool verifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}