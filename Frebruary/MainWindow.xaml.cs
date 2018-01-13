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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;

namespace Frebruary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Browse to the Freb folder";
            dlg.IsFolderPicker = true;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = dlg.FileName;
                // Do something with selected folder string
                locationTextBox.Text = dlg.FileName;

            }

            // Process open file dialog box results

        }

        private void scanButton_Click(object sender, RoutedEventArgs e)
        {
            string location = locationTextBox.Text;
            var ext = new List<string> { "xml" };
            var myFiles = Directory.EnumerateFiles(location, "*.*", SearchOption.AllDirectories)
                 .Where(s => ext.Contains(System.IO.Path.GetExtension(s)));
            /* Populate */

        }
    }
}


public class Author

{

    public string siteId { get; set; }
    public string url { get; set; }
    public string appPoolId { get; set; }
    public string processId { get; set; }
    public string verb { get; set; }
    public string authenticationType { get; set; }
    public string failureReason { get; set; }
    public string statusCode { get; set; }
    public string errorCode { get; set; }

}
/*
 url
siteId
appPoolId
processId
verb
authenticationType
failureReason
statusCode
timeTaken
*/
