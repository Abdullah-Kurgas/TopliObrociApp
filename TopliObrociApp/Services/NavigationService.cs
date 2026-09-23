using Avalonia.Controls;

namespace TopliObrociApp.Services;

public class NavigationService(ContentControl contentControl)
{
    public void Navigate(Control view)
    {
        contentControl.Content = view;
    }
}