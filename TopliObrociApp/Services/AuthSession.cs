using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class AuthSession
{
    private User? _currentUser { get; set; }

    public User? CurrentUser => _currentUser;

    public bool IsAuthenticated =>
        _currentUser != null;

    public void SetUser(User user)
    {
        _currentUser = user;
    }

    public void Logout()
    {
        _currentUser = null;
    }
}