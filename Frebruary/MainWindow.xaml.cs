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
using System.Xml;
using System.Text.RegularExpressions;
using System.Data;

namespace Frebruary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Previewer pre;
        private List<Freb> source;

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
            var ext = new List<string> { ".xml" };
            var myFiles = Directory.EnumerateFiles(location, "*.*", SearchOption.AllDirectories)
                 .Where(s => ext.Contains(System.IO.Path.GetExtension(s)));
            /* Populate */
            List<Freb> source = processedList(myFiles);
            DataGrid.ItemsSource = source;
            
        }

        private List<Freb> processedList(IEnumerable<string> files)
        {
             source = new List<Freb>();
            foreach(var filename in files)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNode node = doc.DocumentElement.SelectSingleNode("/failedRequest");


                XmlNodeList error_node = doc.GetElementsByTagName("freb:Description");
                string val = "", error_info="NA";
                foreach(XmlNode info in error_node)
                {
                    val = info.InnerText;
                    Match match = Regex.Match(val, @"\((0x.*)\d{3,8}\)");
                    if(match.Success)
                    {
                        error_info = match.Value;
                    }
                }
                source.Add(new Freb()
                {
                    siteId = node.Attributes["siteId"]?.InnerText,
                    path = filename,
                    url = node.Attributes["url"]?.InnerText,
                    appPoolId = node.Attributes["appPoolId"]?.InnerText,
                    timeTaken = node.Attributes["timeTaken"]?.InnerText,
                    processId = node.Attributes["processId"]?.InnerText,
                    verb = node.Attributes["verb"]?.InnerText,
                    authenticationType = node.Attributes["authenticationType"]?.InnerText,
                    failureReason = node.Attributes["failureReason"]?.InnerText,
                    statusCode = node.Attributes["statusCode"]?.InnerText,
                    errorCode = error_info
                }
            );
            }

            
            return source;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var row = (Freb)DataGrid.SelectedItem;
            string path = row.path;
            if(pre==null)
            {
                pre = new Previewer(path);
                pre.Show();
            }
            else
            {
                // If already open,bring to focus
                pre.Activate();
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = (Freb)DataGrid.SelectedItem;
            string path = row.path;
            if (pre == null)
            {
                // do nothing if previewer is not found
            }
            else
            {
                // If already open, bring to focus
                pre.path = path;
                pre.reloader();
                pre.Activate();
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Filter(source);
        }
    }
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
