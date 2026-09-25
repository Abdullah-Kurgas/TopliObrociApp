using Avalonia.Controls;
using Avalonia.Interactivity;
using TopliObrociApp.Models;
using TopliObrociApp.Services;
using TopliObrociApp.Windows;

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

    private async void NewUser_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddEditUser();
        if (TopLevel.GetTopLevel(this) is not Window owner) return;

        await window.ShowDialog(owner);
        LoadUsers();
    }

    private async void EditUser_Click(object? sender, RoutedEventArgs e)
    {
        if (UsersDataGrid.SelectedItem is not User user)
            return;

        var window = new AddEditUser(user);

        if (TopLevel.GetTopLevel(this) is Window owner)
            await window.ShowDialog(owner);
        else
            window.Show();
    }
}