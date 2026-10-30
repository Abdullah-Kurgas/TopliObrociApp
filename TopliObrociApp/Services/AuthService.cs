namespace TopliObrociApp.Services;

public class AuthService
{
    public bool Login(string password, bool isAdminLogin)
    {
        return !isAdminLogin || BCrypt.Net.BCrypt.Verify(password, "TODO:....");
    }
}