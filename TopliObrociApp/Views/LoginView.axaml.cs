using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using TopliObrociApp.Models;
using TopliObrociApp.Services;

namespace TopliObrociApp.Views;

public partial class LoginView : UserControl
{
    private readonly AuthService _authService = new();
    private readonly AuthSession _authSession;
    private readonly Action _onLoginSuccess;
    private readonly UserService _userService = new();

    private User? _selectedUser;

    public LoginView(AuthSession authSession, Action showDashboard)
    {
        InitializeComponent();

        _authSession = authSession;
        _onLoginSuccess = showDashboard;

        LoadUsers();
    }

    private void LoadUsers()
    {
        var users = _userService.GetUsers();

        UsersList.ItemsSource = users;

        if (users.Count != 0) UsersList.SelectedIndex = 0;
    }

    private void UsersList_SelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        _selectedUser = UsersList.SelectedItem as User;

        if (_selectedUser == null)
            return;

        SelectedUserName.Text = _selectedUser.FullName;
        SelectedUserInitial.Text = _selectedUser.Initial;

        PasswordBox.Text = string.Empty;

        PasswordBox.IsVisible = !_selectedUser.IgnorePassword;

        ErrorText.Text = string.Empty;

        PasswordBox.Focus();
    }

    private async void LoginButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        var password = PasswordBox.Text ?? "";

        ErrorText.Text = string.Empty;

        if (_selectedUser == null)
        {
            ShowError("Izaberite korisnika.");
            return;
        }

        // Only require a password if the user requires one.
        if (!_selectedUser.IgnorePassword &&
            string.IsNullOrWhiteSpace(password))
        {
            ShowError("Unesite šifru.");
            PasswordBox.Focus();
            return;
        }

        var isAuthenticated = _authService.Login(_selectedUser, password);

        if (!isAuthenticated)
        {
            ShowError("Pogrešna šifra.");
            return;
        }

        await _authSession.SetUserAsync(_selectedUser);

        ErrorText.Foreground = new SolidColorBrush(Color.Parse("#16A34A"));

        ShowError($"Dobrodošli, {_selectedUser.FullName}!");

        _onLoginSuccess();
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.IsVisible = true;
    }
}