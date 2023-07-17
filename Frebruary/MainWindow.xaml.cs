using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data;
using System.Security.Permissions;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Frebruary
{
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Previewer pre;
        public List<Freb> source;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /* Removed use of Windows Core API Code Pack
             * 
             * var dlg = new CommonOpenFileDialog();
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
            */
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                FolderBrowserDialog fbroswer = new System.Windows.Forms.FolderBrowserDialog(); ;
                System.Windows.Forms.DialogResult result = fbroswer.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    locationTextBox.Text = fbroswer.SelectedPath;
                }

            }
            // Process open file dialog box results

        }
        private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                scanButton_Click(sender, e);
            }
        }
        public void scanButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string location = locationTextBox.Text;
                var ext = new List<string> { ".xml" };
                var myFiles = Directory.EnumerateFiles(location, "*.*", SearchOption.AllDirectories)
                     .Where(s => ext.Contains(System.IO.Path.GetExtension(s)));
                /* Populate */
                List<Freb> source = processedList(myFiles);
                DataGrid.ItemsSource = source;
            }
            catch(System.UnauthorizedAccessException)
            {
                System.Windows.MessageBox.Show("Could not scan the directory. Insufficient privileges to open the folder.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                System.Windows.MessageBox.Show("Could not scan the directory. Directory not found.");
            }
            catch (Exception e1)
            {
                System.Windows.MessageBox.Show("Could not scan the directory. \nException: "+e1.ToString());
            }
        }

        private List<Freb> processedList(IEnumerable<string> files)
        {
             source = new List<Freb>();
            foreach(var filename in files)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNode node = doc.DocumentElement.SelectSingleNode("/failedRequest");


                XmlNodeList error_node = doc.GetElementsByTagName("Data");
                Console.Write(error_node.Count);
                string val = "", error_info="NA";
                foreach(XmlNode info in error_node)
                {
                    val = info.InnerText;
                    if (val == "0") continue;
                    //Match match = Regex.Match(val, @"\((0x.*)\d{3,8}\)");
                    // New error code regex : .*ErrorCode.*(\d[10]).*
                    //Match match = Regex.Match(val, @"ErrorCode.*(\d{10})");
                    /*
                    if (match.Success)
                    {
                        error_info = match.Value;
                    }*/
                    if(info.Attributes!= null)
                    {
                        if(info.Attributes["Name"]!=null)
                        {
                            if(info.Attributes["Name"].Value=="ErrorCode")
                                error_info = val;
                        }
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
            if(row == null)
            {
                System.Windows.MessageBox.Show("Click on a Freb to load the previewer");
                return;
            }
            string path = row.path;
            if(pre == null)
            {
                pre = new Previewer(path);
                pre.Show();
            }
            else
            {
                if (pre.IsLoaded == false)
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
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = (Freb)DataGrid.SelectedItem;
            if (row == null) return;
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
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(source == null)
            {
                System.Windows.MessageBox.Show("Scan and populate the Freb entries before applying a filter");
                return;
            }
            Filter fil = new Filter(this);
            fil.ShowInTaskbar = false;
            fil.Owner = this;
            fil.ShowDialog();
        }

        public void reload()
        {
            DataGrid.ItemsSource = source;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (File.Exists(@"filter.xml"))
            {
                File.Delete(@"filter.xml");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //do own transform
            const String filename = "D:\\temp\\freb\\fr000001.xml";
            const String stylesheet = "D:\\temp\\freb\\freb.xsl";

            // Compile the style sheet.
            XsltSettings xslt_settings = new XsltSettings();
            xslt_settings.EnableScript = true;
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(stylesheet, xslt_settings, new XmlUrlResolver());

            // Load the XML source file.
            XPathDocument doc = new XPathDocument(filename);

            /*
            // Create an XmlWriter.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create("output.xml", settings);

            // Execute the transformation.
            xslt.Transform(doc, writer);
            writer.Close();
            */



            //Create an XmlTextWriter to output to the console.               
            XmlTextWriter writer = new XmlTextWriter(Console.Out);
            writer.Formatting = Formatting.Indented;

            //Transform the file.  
            xslt.Transform(doc, writer);
            writer.Close();

 //           About a = new About();
 //           a.Show();
        }
    }
}
