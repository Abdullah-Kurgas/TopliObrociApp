using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using TopliObrociApp.Models;

namespace TopliObrociApp.Views.Controls;

public partial class UpdateNotification : UserControl
{
    public UpdateNotification()
    {
        InitializeComponent();
        IsVisible = false;
    }

    public void ShowUpdate(UpdateInfo update)
    {
        UpdateTitle.Text = $"Nova verzija {update.Version} je dostupna";
        UpdateDescription.Text = update.ReleaseNotes;

        IsVisible = true;
    }

    private void CloseApplication_Click(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime
            is IClassicDesktopStyleApplicationLifetime lifetime)
            lifetime.Shutdown();
    }

    private void Dismiss_Click(object? sender, RoutedEventArgs e)
    {
        IsVisible = false;
    }
}