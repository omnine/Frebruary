using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;

namespace Frebruary
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Filter : Window
    {
        List<Freb> originalSource;
        MainWindow _parent;

        public Filter(MainWindow form)
        {
            InitializeComponent();
            originalSource = form.source;
            _parent = form;
            populateComboBox();
            disabler();
            loadPreviousValues();
        }


        public void populateComboBox()
        {
            List<string> siteId = new List<string>();
            List<string> appPoolId = new List<string>();
            List<string> verb = new List<string>();
            List<string> authenticationType = new List<string>();
            foreach (var obj in originalSource)
            {
                siteId.Add(obj.siteId);
                appPoolId.Add(obj.appPoolId);
                verb.Add(obj.verb);
                authenticationType.Add(obj.authenticationType);
            }
            siteIdComboBox.ItemsSource = siteId.Select(o => o).Distinct().ToList();
            appPoolIdComboBox.ItemsSource = appPoolId.Select(o => o).Distinct().ToList();
            verbComboBox.ItemsSource = verb.Select(o => o).Distinct().ToList();
            authTypeComboBox.ItemsSource = authenticationType.Select(o => o).Distinct().ToList();
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            disabler();
        }
        
        public void loadPreviousValues()
        {
            if (File.Exists("filter.xml"))
            {
                /* We must first populate comboBox, else those values wont be retained */ 
                string xmlStr = System.IO.File.ReadAllText(@"filter.xml"); ;
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlStr)))
                {
                    while (reader.Read())
                    {
                        // Only detect start elements.
                        if (reader.IsStartElement())
                        {
                            // Get element name and switch on it.
                            switch (reader.Name)
                            {
                                case "siteId":
                                    siteIdCheckBox.IsChecked = true;
                                    siteIdComboBox.Text = reader["value"];
                                    break;
                                case "url":
                                    urlCheckBox.IsChecked = true;
                                    urlTextBox.Text = reader["value"];
                                    break;
                                case "appPoolId":
                                    appPoolIdCheckBox.IsChecked = true;
                                    appPoolIdComboBox.Text = reader["value"];
                                    break;
                                case "verb":
                                    verbCheckBox.IsChecked = true;
                                    verbComboBox.Text = reader["value"];
                                    break;
                                case "statusCode":
                                    statusCodeCheckBox.IsChecked = true;
                                    statusTextBox.Text = reader["value"];
                                    break;
                                case "errorCode":
                                    errorCodeCheckBox.IsChecked = true;
                                    errorCodeTextBox.Text = reader["value"];
                                    break;
                                case "timeTaken":
                                    timeTakenCheckBox.IsChecked = true;
                                    timeTakenTextBox.Text = reader["value"];
                                    break;
                                case "processId":
                                    processIdCheckBox.IsChecked = true;
                                    processIdTextBox.Text = reader["value"];
                                    break;
                                case "authenticationType":
                                    authTypeCheckBox.IsChecked = true;
                                    authTypeComboBox.Text = reader["value"];
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public void disabler()
        {

            if (siteIdCheckBox.IsChecked == true) { siteIdComboBox.IsEnabled = true; } else { siteIdComboBox.IsEnabled = false; }
            if (urlCheckBox.IsChecked == true) { urlTextBox.IsEnabled = true; } else { urlTextBox.IsEnabled = false; }
            if (appPoolIdCheckBox.IsChecked == true) { appPoolIdComboBox.IsEnabled = true; } else { appPoolIdComboBox.IsEnabled = false; }
            if (verbCheckBox.IsChecked == true) { verbComboBox.IsEnabled = true; } else { verbComboBox.IsEnabled = false; }
            if (statusCodeCheckBox.IsChecked == true) { statusTextBox.IsEnabled = true; } else { statusTextBox.IsEnabled = false; }
            if (errorCodeCheckBox.IsChecked == true) { errorCodeTextBox.IsEnabled = true; } else { errorCodeTextBox.IsEnabled = false; }
            if (timeTakenCheckBox.IsChecked == true) { timeTakenTextBox.IsEnabled = true; } else { timeTakenTextBox.IsEnabled = false; }
            if (processIdCheckBox.IsChecked == true) { processIdTextBox.IsEnabled = true; } else { processIdTextBox.IsEnabled = false; }
            if (authTypeCheckBox.IsChecked == true) { authTypeComboBox.IsEnabled = true; } else { authTypeComboBox.IsEnabled = false; }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /* Apply Filter */
            string siteVal = "" , urlVal = "", appVal = "", verbVal = "", statusVal = "", errorVal = "", timeVal = "", processVal = "", authVal = "";

            if (siteIdCheckBox.IsChecked == true) { siteVal = siteIdComboBox.Text; }
            if (urlCheckBox.IsChecked == true) { urlVal = urlTextBox.Text; }
            if (appPoolIdCheckBox.IsChecked == true) { appVal = appPoolIdComboBox.Text; }
            if (verbCheckBox.IsChecked == true) { verbVal = verbComboBox.Text; }
            if (statusCodeCheckBox.IsChecked == true) { statusVal = statusTextBox.Text; }
            if (errorCodeCheckBox.IsChecked == true) { errorVal = errorCodeTextBox.Text; }
            if (timeTakenCheckBox.IsChecked == true) { timeVal = timeTakenTextBox.Text; }
            if (processIdCheckBox.IsChecked == true) { processVal = processIdTextBox.Text; }
            if (authTypeCheckBox.IsChecked == true) { authVal = authTypeComboBox.Text; }

            var results = originalSource.Where( x =>
                 (siteVal == "" ? true : x.siteId == siteVal)    &&
                 (urlVal == "" ? true : x.url == urlVal)       &&
                 (appVal == "" ? true : x.appPoolId == appVal)   &&
                 (verbVal == "" ? true : x.verb == verbVal)     &&
                 (statusVal == "" ? true : x.statusCode == statusVal)   &&
                 (errorVal == "" ? true : x.errorCode == errorVal)    &&
                 (timeVal == "" ? true : x.timeTaken == timeVal)      &&
                 (processVal == "" ? true : x.processId == processVal)  &&
                 (authVal == "" ? true : x.authenticationType == authVal) 
                ).ToList<Freb>();

            /* Save these settings to a file */
            using (XmlWriter writer = XmlWriter.Create("filter.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("filterSettings");

                if (siteIdCheckBox.IsChecked == true) { writer.WriteStartElement("siteId"); writer.WriteAttributeString("value", siteVal); writer.WriteEndElement(); }
                if (urlCheckBox.IsChecked == true) { writer.WriteStartElement("url"); writer.WriteAttributeString("value", urlVal); writer.WriteEndElement(); }
                if (appPoolIdCheckBox.IsChecked == true) { writer.WriteStartElement("appPoolId"); writer.WriteAttributeString("value", appVal); writer.WriteEndElement(); }
                if (verbCheckBox.IsChecked == true) { writer.WriteStartElement("verb"); writer.WriteAttributeString("value", verbVal); writer.WriteEndElement(); }
                if (statusCodeCheckBox.IsChecked == true) { writer.WriteStartElement("statusCode"); writer.WriteAttributeString("value", statusVal); writer.WriteEndElement(); }
                if (errorCodeCheckBox.IsChecked == true) { writer.WriteStartElement("errorCode"); writer.WriteAttributeString("value", errorVal); writer.WriteEndElement(); }
                if (timeTakenCheckBox.IsChecked == true) { writer.WriteStartElement("timeTaken"); writer.WriteAttributeString("value", timeVal); writer.WriteEndElement(); }
                if (processIdCheckBox.IsChecked == true) { writer.WriteStartElement("processId"); writer.WriteAttributeString("value", processVal); writer.WriteEndElement(); }
                if (authTypeCheckBox.IsChecked == true) { writer.WriteStartElement("authenticationType"); writer.WriteAttributeString("value", authVal); writer.WriteEndElement(); }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            /* Update the Main Window Source */
            _parent.source = results;
            _parent.reload();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (File.Exists(@"filter.xml"))
            {
                File.Delete(@"filter.xml");
            }
            MessageBox.Show("Removing all applied filters");
            _parent.scanButton_Click(sender, e);
            this.Close();
        }
    }
}
