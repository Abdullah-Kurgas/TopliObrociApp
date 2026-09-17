using Avalonia.Controls;
using TopliObrociApp.Views;

namespace TopliObrociApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ShowTopliObrokView();
    }

    private void ShowLogin()
    {
        MainContent.Content = new LoginView(ShowTopliObrokView);
    }

    private void ShowTopliObrokView()
    {
        MainContent.Content = new TopliObrokView();
    }
}