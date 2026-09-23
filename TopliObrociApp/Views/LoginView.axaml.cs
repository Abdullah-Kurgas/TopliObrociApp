using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TopliObrociApp.Services;

namespace TopliObrociApp.Views;

public partial class LoginView : UserControl
{
    private readonly AuthService _authService = new();
    private readonly AuthSession _authSession;
    private readonly Action _onLoginSuccess;

    public LoginView(AuthSession authSession, Action showDashboard)
    {
        InitializeComponent();
        _authSession = authSession;
        _onLoginSuccess = showDashboard;
    }

    private void LoginButton_Click(object? sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text?.Trim() ?? "";
        var password = PasswordTextBox.Text ?? "";

        ErrorText.IsVisible = false;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ShowError("Unesite username i password");
            return;
        }

        try
        {
            var user = _authService.Login(username, password);

            if (user is null)
            {
                ShowError("Pogrešan username ili password");
                return;
            }

            _authSession.SetUser(user);
            _onLoginSuccess();
        }
        catch (Exception ex)
        {
            ShowError($"Greška: {ex.Message}");
        }
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.IsVisible = true;
    }
}