using System.Windows;

namespace Frebruary
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        MainWindow _parent;
        public About(MainWindow form)
        {
            InitializeComponent();
            _parent = form;
            radioWebBrowser.IsChecked = !_parent.bUseWebView;
            radioWebView.IsChecked = _parent.bUseWebView;
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            _parent.bUseWebView = (bool)radioWebView.IsChecked;
            this.Close();
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
