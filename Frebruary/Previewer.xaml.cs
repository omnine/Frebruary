using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]

    public partial class Previewer : Window
    {
        public string path;


        private async Task InitializeAsync(Uri s)
        {
            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--allow-file-access-from-files");
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);
            await browser2.EnsureCoreWebView2Async(environment);


//            browser2.CoreWebView2.Navigate("https://yourlink.jnlp");
            browser2.Source = s;
        }

        public Previewer(string spath)
        {
            InitializeComponent();
            Uri s;
            if (spath=="" || spath == null)
            {
            }else
            {
                path = spath;
                s = new Uri(path);
                browser.Source = s;
                this.Title = "Previewer - "+path;

                _ = InitializeAsync(s);
            }
        }

        public void reloader()
        {
            Uri s = new Uri(path);
            browser.Source = s;
            this.Title = "Previewer - " + path;

        }
    }
}
