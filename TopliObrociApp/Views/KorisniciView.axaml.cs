using Avalonia.Controls;
using TopliObrociApp.Services;

namespace TopliObrociApp.Views;

public partial class KorisniciView : UserControl
{
    private readonly UserService _userService = new();
    private AuthSession _authSession;

    public KorisniciView(AuthSession authSession)
    {
        InitializeComponent();
        LoadUsers();

        _authSession = authSession;
    }

    private void LoadUsers()
    {
        var users = _userService.GetUsers();
        UsersDataGrid.ItemsSource = users;
    }
}