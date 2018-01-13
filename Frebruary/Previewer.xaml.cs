using System;
using System.Collections.Generic;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Previewer : Window
    {
        public string path;

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
