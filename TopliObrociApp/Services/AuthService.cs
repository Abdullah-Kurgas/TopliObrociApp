using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class AuthService
{
    public bool Login(User user, string password)
    {
        if (!user.IsActive) return false;

        return user.IgnorePassword || BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}