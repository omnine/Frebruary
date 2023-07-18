using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

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


        private async Task InitializeAsync(string html)
        {
            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--allow-file-access-from-files");
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);
            await browser2.EnsureCoreWebView2Async(environment);

            browser2.CoreWebView2.NavigateToString(html);
//            browser2.CoreWebView2.Navigate("https://yourlink.jnlp");
//            browser2.Source = s;
        }

        public Previewer(string spath, bool bUseWebView)
        {
            InitializeComponent();
            if (spath=="" || spath == null)
            {
                return;
            }

            path = spath;
            if (bUseWebView)
            {
                browser.Visibility = Visibility.Hidden;
                browser2.Visibility = Visibility.Visible;

                const String stylesheet = "D:\\temp\\freb\\freb.xsl";

                // Compile the style sheet.
                XsltSettings xslt_settings = new XsltSettings();
                xslt_settings.EnableScript = true;
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(stylesheet, xslt_settings, new XmlUrlResolver());

                // Load the XML source file.
                XPathDocument doc = new XPathDocument(spath);


                // Create an XmlWriter.
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;


                using (var ms = new MemoryStream())
                using (var writer = new XmlTextWriter(ms, new UTF8Encoding(false))
                { Formatting = Formatting.Indented })
                {
                    // Execute the transformation.
                    xslt.Transform(doc, writer);
                    string html = Encoding.UTF8.GetString(ms.ToArray());
                    _ = InitializeAsync(html);
                    //                   browser2.NavigateToString(html);
                    //                   Console.Write(html);
                }


            }
            else
            {
                browser2.Visibility = Visibility.Hidden;
                browser.Visibility = Visibility.Visible;
                Uri s = new Uri(path);
                browser.Source = s;
                this.Title = "Previewer - " + path;
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
