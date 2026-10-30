using System;
using Avalonia.Controls;
using Avalonia.Input;
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

        UserInitial.Text = _authSession.CurrentUser?.Initial ?? "?";
        UserFullName.Text = _authSession.CurrentUser?.FullName ?? "?";

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

    private void Radnici_Click(object? sender, RoutedEventArgs e)
    {
        _navigation.Navigate(new KorisniciView());
    }

    private void ApplyRolePermissions()
    {
        var role = _authSession.CurrentUser!.Role;

        RadniciNav.IsVisible = role == Role.Administrator;
        PostavkeBtn.IsVisible = role == Role.Administrator;
    }

    private async void Odjava_Click(object? sender, RoutedEventArgs e)
    {
        await _authSession.LogoutAsync();
        Environment.Exit(0);
    }

    private void DoubleTapped(object? sender, TappedEventArgs e)
    {
        OdjavaBtn.IsVisible = !OdjavaBtn.IsVisible;
    }
}