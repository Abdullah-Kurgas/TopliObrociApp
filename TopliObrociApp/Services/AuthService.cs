using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class AuthService
{
    private readonly UserService _userService = new();

    public User? Login(string username, string password)
    {
        var user = _userService.GetUserByUsername(username);

        if (user is null) return null;
        if (!user.IsActive) return null;

        var passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        return !passwordValid ? null : user;
    }
}