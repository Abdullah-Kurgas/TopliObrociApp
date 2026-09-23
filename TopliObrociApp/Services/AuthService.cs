using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class AuthService
{
    private readonly UserService _userService = new();

    public bool Login(User user, string password)
    {
        if (!user.IsActive) return false;
        var passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        return passwordValid;
    }
}