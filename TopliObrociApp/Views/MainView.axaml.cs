using Avalonia.Controls;
using Avalonia.Interactivity;
using TopliObrociApp.Enums;
using TopliObrociApp.Services;

namespace TopliObrociApp.Views;

public partial class MainView : UserControl
{
    private readonly AuthSession _authSession;
    private readonly NavigationService _navigation;

    public MainView(AuthSession authSession)
    {
        InitializeComponent();

        _authSession = authSession;
        _navigation = new NavigationService(ContentArea);

        ApplyRolePermissions();
        ShowTopliObrok();
    }

    private void ShowTopliObrok()
    {
        _navigation.Navigate(new TopliObrokView(_authSession));
    }

    private void TopliObrok_Click(object? sender, RoutedEventArgs e)
    {
        ShowTopliObrok();
    }

    private void Korisnici_Click(object? sender, RoutedEventArgs e)
    {
        _navigation.Navigate(new KorisniciView(_authSession));
    }

    private void ApplyRolePermissions()
    {
        var role = _authSession.CurrentUser!.Role;

        KorisniciNav.IsVisible = role == Role.Administrator;
    }
}