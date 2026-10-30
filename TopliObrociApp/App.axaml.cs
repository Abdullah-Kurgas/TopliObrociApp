using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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

            var mainWindow = new MainWindow();
            desktop.MainWindow = mainWindow;
            mainWindow.Show();

            splashScreen.Close();
        }

        base.OnFrameworkInitializationCompleted();
    }
}