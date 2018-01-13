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
    public partial class Filter : Window
    {
        List<Freb> originalSource;
        public Filter(List<Freb> source)
        {
            InitializeComponent();
            originalSource = source;
            populateComboBox();
            disabler();
        }

        public List<string> distinct(List<string> param)
        {
            List<string> result = null;
            param.ForEach(x =>
            {
                if (result.Contains(x))
                {

                } else
                {
                    result.Add(x);
                }
            });
            return result;
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
    }
}
