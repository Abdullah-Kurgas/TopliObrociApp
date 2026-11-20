using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace TopliObrociApp.Views.Controls;

public partial class MainNavigation : UserControl
{
    public MainNavigation()
    {
        InitializeComponent();
    }

    public event EventHandler? TopliObrokRequested;
    public event EventHandler? RadniRasporedRequested;
    public event EventHandler? RadniciRequested;
    public event EventHandler? LogoutRequested;

    public void SetUser(string initial, string fullName)
    {
        UserInitial.Text = initial;
        UserFullName.Text = fullName;
    }

    public void SetRadniciVisibility(bool visible)
    {
        RadniciNav.IsVisible = visible;
    }

    private void Logout_Click(object? sender, RoutedEventArgs e)
    {
        LogoutRequested?.Invoke(this, EventArgs.Empty);
    }

    private void TopliObrok_Click(object? sender, RoutedEventArgs e)
    {
        TopliObrokRequested?.Invoke(this, EventArgs.Empty);
    }

    private void Radniraspored_Click(object? sender, RoutedEventArgs e)
    {
        RadniRasporedRequested?.Invoke(this, EventArgs.Empty);
    }

    private void Radnici_Click(object? sender, RoutedEventArgs e)
    {
        RadniciRequested?.Invoke(this, EventArgs.Empty);
    }

    private void DoubleTapped(object? sender, TappedEventArgs e)
    {
        OdjavaBtn.IsVisible = !OdjavaBtn.IsVisible;
    }
}