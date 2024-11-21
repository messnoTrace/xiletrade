using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xiletrade.Library.ViewModels;

namespace Xiletrade.Views;

/// <summary>
/// Logique d'interaction pour StartWindow.xaml
/// </summary>
public partial class StartView : ViewBase
{
    public StartView()
    {
        InitializeComponent();
        MouseLeftButtonDown += Window_DragWindow;
    }

    private void Window_DragWindow(object sender, MouseButtonEventArgs e)
    {
        this.DragMove();
    }

    private void OnCookieTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (sender is TextBox cookieBox) {
            var viewModel = (StartViewModel)DataContext;
            viewModel.CookieStr = cookieBox.Text;
        }
      
    }
}
