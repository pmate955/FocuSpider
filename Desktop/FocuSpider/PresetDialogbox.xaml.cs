using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FocuSpider
{
    /// <summary>
    /// Interaction logic for PresetDialogbox.xaml
    /// </summary>
    public partial class PresetDialogbox : Window
    {
        public PresetDialogbox(Preset p)
        {
            InitializeComponent();
            this.txtName.Text = p.Name;
            this.txtSteps.Text = p.Steps.ToString();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public string Name { get { return this.txtName.Text; } }

        public int Steps { get { return Convert.ToInt32(this.txtSteps.Text); } }
    }
}
