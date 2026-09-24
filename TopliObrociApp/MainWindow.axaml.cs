using Avalonia.Controls;
using TopliObrociApp.Services;
using TopliObrociApp.Views;

namespace TopliObrociApp;

public partial class MainWindow : Window
{
    private readonly AuthSession _authSession;
    private readonly NavigationService _navigation;

    public MainWindow()
    {
        InitializeComponent();

        _authSession = new AuthSession();
        _navigation = new NavigationService(MainContent);

        ShowLogin();
    }

    private void ShowLogin()
    {
        _navigation.Navigate(new LoginView(_authSession, ShowMainView));
    }

    private void ShowMainView()
    {
        _navigation.Navigate(new MainView(_authSession));
    }
}