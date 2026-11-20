using System;
using Avalonia.Controls;
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

        MainNavigationControl.SetUser(
            _authSession.CurrentUser?.Initial ?? "?",
            _authSession.CurrentUser?.FullName ?? "?"
        );

        MainNavigationControl.TopliObrokRequested += (_, _) =>
            ShowTopliObrok();

        MainNavigationControl.RadniRasporedRequested += (_, _) =>
            _navigation.Navigate(new RadniRasporedView());

        MainNavigationControl.RadniciRequested += (_, _) =>
            _navigation.Navigate(new KorisniciView());

        MainNavigationControl.LogoutRequested += Odjava_Click;

        ApplyRolePermissions();

        ShowTopliObrok();
    }

    private void ShowTopliObrok()
    {
        _navigation.Navigate(new TopliObrokView(_authSession));
    }

    private void ApplyRolePermissions()
    {
        var role = _authSession.CurrentUser!.Role;

        MainNavigationControl.SetRadniciVisibility(
            role == Role.Administrator
        );
    }

    private async void Odjava_Click(object? sender, EventArgs eventArgs)
    {
        await _authSession.LogoutAsync();
        Environment.Exit(0);
    }
}