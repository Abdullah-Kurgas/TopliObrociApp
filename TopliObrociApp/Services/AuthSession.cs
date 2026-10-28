using System.Threading.Tasks;
using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class AuthSession
{
    private readonly AuthSessionStorage _storage = new();

    public User? CurrentUser { get; private set; }

    public bool IsAuthenticated => CurrentUser != null;

    public async Task SetUserAsync(User user)
    {
        CurrentUser = user;

        await _storage.SaveSessionAsync(user);
    }

    public async Task<bool> RestoreSessionAsync()
    {
        var user = await _storage.LoadSessionAsync();

        if (user == null) return false;

        CurrentUser = user;
        return true;
    }

    public async Task LogoutAsync()
    {
        CurrentUser = null;

        await _storage.ClearSessionAsync();
    }
}