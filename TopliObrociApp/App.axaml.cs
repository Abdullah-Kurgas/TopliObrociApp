using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TopliObrociApp.Configuration;
using TopliObrociApp.Windows;

namespace TopliObrociApp;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var splashScreen = new SplashScreen();
            desktop.MainWindow = splashScreen;
            splashScreen.Show();

            try
            {
                var mounted = await AppSettings.EnsureUsersShareMountedAsync();

                if (!mounted) throw new Exception("Nije dohvacena putanja ka users.json");
            }
            catch (Exception e)
            {
                return;
            }

            var mainWindow = new MainWindow();
            desktop.MainWindow = mainWindow;
            mainWindow.Show();

            splashScreen.Close();
        }

        base.OnFrameworkInitializationCompleted();
    }
}