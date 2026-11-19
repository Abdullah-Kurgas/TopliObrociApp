using Avalonia.Controls;
using TopliObrociApp.Services;
using TopliObrociApp.Views;

namespace TopliObrociApp;

public partial class MainWindow : Window
{
    private readonly AuthSession _authSession;
    private readonly NavigationService _navigation;
    private readonly UpdateService _updateService;

    public MainWindow()
    {
        InitializeComponent();

        _authSession = new AuthSession();
        _navigation = new NavigationService(MainContent);
        _updateService = new UpdateService();

        OpenApplicationAsync();
        CheckForUpdateAsync();
    }

    private async void OpenApplicationAsync()
    {
        var restored = await _authSession.RestoreSessionAsync();

        if (restored)
            ShowMainView();
        else
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

    private async void CheckForUpdateAsync()
    {
        var update = await _updateService.CheckForUpdateAsync();

        if (update == null)
            return;

        UpdateNotificationPanel.ShowUpdate(update);
    }
}