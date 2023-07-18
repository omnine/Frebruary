using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            this.Hide();
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
