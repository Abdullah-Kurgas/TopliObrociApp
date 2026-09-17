using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TopliObrociApp.Views;

public partial class LoginView : UserControl
{
    private readonly Action _onLoginSuccess;

    public LoginView(Action showDashboard)
    {
        InitializeComponent();

        _onLoginSuccess = showDashboard;
    }

    private void LoginButton_Click(object? sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text ?? "";
        var password = PasswordTextBox.Text ?? "";

        if (username == "pero" && password == "1234")
            _onLoginSuccess();
        else
            ErrorTextBlock.Text = "Pogrešno korisničko ime ili lozinka.";
    }
}