using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TopliObrociApp.Enums;
using TopliObrociApp.Models;
using TopliObrociApp.Services;

namespace TopliObrociApp.Views;

public partial class LoginView : UserControl
{
    private readonly AuthService _authService = new();
    private readonly AuthSession _authSession;
    private readonly GarsonService _garsonService = new();

    private readonly bool _isAdminLogin = false;
    private readonly Action _onLoginSuccess;
    private CancellationTokenSource? _autocompleteCts;

    private User? _selectedUser;

    public LoginView(AuthSession authSession, Action showDashboard)
    {
        InitializeComponent();

        _authSession = authSession;
        _onLoginSuccess = showDashboard;

        ReprezentAutoComplete.PropertyChanged += ReprezentAutoComplete_PropertyChanged;
    }

    private async void ReprezentAutoComplete_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var searchText = ReprezentAutoComplete.Text?.Trim();

        _autocompleteCts?.Cancel();

        var cts = new CancellationTokenSource();
        _autocompleteCts = cts;

        try
        {
            await Task.Delay(350, cts.Token);

            if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 2)
            {
                if (ReferenceEquals(_autocompleteCts, cts)) ReprezentAutoComplete.ItemsSource = null;
                return;
            }

            var results = await _garsonService.GetReprezenteByNameAsync(searchText);

            if (cts.IsCancellationRequested || !ReferenceEquals(_autocompleteCts, cts))
                return;

            ReprezentAutoComplete.ItemsSource = results;
            ReprezentAutoComplete.IsDropDownOpen = results.Count > 0;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine(searchText);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
        finally
        {
            if (ReferenceEquals(_autocompleteCts, cts)) _autocompleteCts = null;

            cts.Dispose();
        }
    }

    private void ReprezentAutoComplete_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != AutoCompleteBox.SelectedItemProperty) return;
        if (e.NewValue is not CReprezent reprezent) return;

        var parts = reprezent.Ime.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return;

        var lastName = parts[0];
        var firstName = string.Join(" ", parts.Skip(1));

        _selectedUser = new User
        {
            GarsonId = reprezent.IdReprezenta,
            FirstName = firstName,
            LastName = lastName,
            Role = OdaberiUserRolu()
        };

        SelectedUserName.Text = _selectedUser.FullName;
        SelectedUserInitial.Text = _selectedUser.Initial;

        ReprezentAutoComplete.Text = null;
        ReprezentAutoComplete.IsVisible = false;
        LoginButton.IsVisible = true;
    }

    private Role OdaberiUserRolu()
    {
        return Role.ObicniKorisnik;
    }

    private async void LoginButton_Click(object? sender, RoutedEventArgs e)
    {
        var password = PasswordBox.Text ?? "";

        ErrorText.Text = string.Empty;

        if (_selectedUser == null)
        {
            ShowError("Izaberite korisnika.");
            return;
        }

        // Only require a password if it is admin login.
        if (_isAdminLogin && string.IsNullOrWhiteSpace(password))
        {
            ShowError("Unesite šifru.");
            PasswordBox.Focus();
            return;
        }

        var isAuthenticated = _authService.Login(password, _isAdminLogin);

        if (!isAuthenticated)
        {
            ShowError("Pogrešna šifra.");
            return;
        }

        await _authSession.SetUserAsync(_selectedUser);
        _onLoginSuccess();
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.IsVisible = true;
    }
}