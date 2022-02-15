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
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;

namespace FocuSpider
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Focuser _focuser;

        public MainWindow()
        {
            InitializeComponent();
            this._setEnabledInputs(false);
            this.Title = "FocuSpider";
            this._focuser = new Focuser();
            this._setSerialPorts();
            SonyWindowClicker.DoClick();
        }

        private void _setSerialPorts()
        {
            cbSerialPort.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (var port in ports) cbSerialPort.Items.Add(port);
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (Connect.Content.ToString() == "Connect")
            {
                string portName = this.cbSerialPort.SelectedItem as string;
                bool isSuccess = this._focuser.TryConnect(portName);

                if (isSuccess)
                {
                    this._setEnabledInputs(true);
                    this.Connect.Content = "Disconnect";
                }
            }
            else
            {
                bool isSuccess = this._focuser.Disconnect();

                if (isSuccess)
                {
                    this._setEnabledInputs(false);
                    this.Connect.Content = "Connect";
                }
            }
        }

        private void _setEnabledInputs(bool isEnabled)
        {
            this.slStepCount.IsEnabled = isEnabled;
            this.btnLLeft.IsEnabled = isEnabled;
            this.btnLeft.IsEnabled = isEnabled;
            this.btnRight.IsEnabled = isEnabled;
            this.btnRRight.IsEnabled = isEnabled;
            this.btnToStart.IsEnabled = isEnabled;
            this.btnToEnd.IsEnabled = isEnabled;
            this.btnStart.IsEnabled = isEnabled;
        }

        private void PictureCountValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+");
            bool isWrong = !(regex.IsMatch(e.Text) && this.txtPictureCount.Text.Length < 3);
            e.Handled = isWrong;
        }

        private async void btnRight_Click(object sender, RoutedEventArgs e)
        {
            this._setEnabledInputs(false);
            this.btnRight.Foreground = Brushes.Red;
            await this._focuser.Step(true, Convert.ToInt32(this.slStepCount.Value));
            this.btnRight.Foreground = Brushes.Black;
            this._setEnabledInputs(true);
        }

        private async void btnRRight_Click(object sender, RoutedEventArgs e)
        {
            this._setEnabledInputs(false);
            this.btnRRight.Foreground = Brushes.Red;
            await this._focuser.Step(true, Convert.ToInt32(this.slStepCount.Value) * 2);
            this.btnRRight.Foreground = Brushes.Black;
            this._setEnabledInputs(true);
        }

        private async void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            this._setEnabledInputs(false);
            this.btnLeft.Foreground = Brushes.Red;
            await this._focuser.Step(false, Convert.ToInt32(this.slStepCount.Value));
            this.btnLeft.Foreground = Brushes.Black;
            this._setEnabledInputs(true);
        }

        private async void btnLLeft_Click(object sender, RoutedEventArgs e)
        {
            this._setEnabledInputs(false);
            this.btnLLeft.Foreground = Brushes.Red;
            await this._focuser.Step(false, Convert.ToInt32(this.slStepCount.Value) * 2);
            this.btnLLeft.Foreground = Brushes.Black;
            this._setEnabledInputs(true);
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this._setEnabledInputs(false);
            int pictureCount = Convert.ToInt32(this.txtPictureCount.Text);
            int seconds = Convert.ToInt32(this.txtTimeout.Text);
            int steps = Convert.ToInt32(this.slStepCount.Value);


            await Task.Run(async () =>
            {

                for (int i = 0; i < pictureCount; i++)
                {
                    this.btnStart.Dispatcher.Invoke(() =>
                    {
                        btnStart.Content = $"Images: {i + 1}";
                    });
                    await this._focuser.Step(true, steps);
                    await Task.Delay(seconds * 1000);
                }
            });

            this.btnStart.Content = "Start";
            this._setEnabledInputs(true);

        }
    }
}
