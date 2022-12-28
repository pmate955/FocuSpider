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
using System.Runtime.InteropServices;
using System.Configuration;
using System.ComponentModel;

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
            this.Title = "FocuSpider";
            this._setEnabledInputs(false);
            this.btnStart.IsEnabled = false;
            this._focuser = new Focuser();
            this._setSerialPorts();
            this.cbSerialPort.DropDownOpened += this.cbSerialportsDropDown;
            this._isCapturing = false;
            this.Topmost = cbAlwaysOnTop.IsChecked.GetValueOrDefault();
            this.loadState();
            Closing += MainWindow_Closing;
        }

        private bool _isCapturing;

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
                    this.btnStart.IsEnabled = true;
                    this.Connect.Content = "Disconnect";
                }
            }
            else
            {
                bool isSuccess = this._focuser.Disconnect();

                if (isSuccess)
                {
                    this.btnStart.IsEnabled = false;
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
            this.PresetA.IsEnabled = isEnabled;
            this.PresetB.IsEnabled = isEnabled;
            this.PresetC.IsEnabled = isEnabled;
            this.PresetD.IsEnabled = isEnabled;
            //this.btnStart.IsEnabled = isEnabled;
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
            await this._focuser.StepAsync(true, Convert.ToInt32(this.slStepCount.Value));
            this.btnRight.Foreground = Brushes.Black;
            this._setEnabledInputs(true);
        }

        private async void btnRRight_Click(object sender, RoutedEventArgs e)
        {
            this._setEnabledInputs(false);
            this.btnRRight.Foreground = Brushes.Red;
            await this._focuser.StepAsync(true, Convert.ToInt32(this.slStepCount.Value) * 2);
            this.btnRRight.Foreground = Brushes.Black;
            this._setEnabledInputs(true);
        }

        private async void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            this._setEnabledInputs(false);
            this.btnLeft.Foreground = Brushes.Red;
            await this._focuser.StepAsync(false, Convert.ToInt32(this.slStepCount.Value));
            this.btnLeft.Foreground = Brushes.Black;
            this._setEnabledInputs(true);
        }

        private async void btnLLeft_Click(object sender, RoutedEventArgs e)
        {
            this._setEnabledInputs(false);
            this.btnLLeft.Foreground = Brushes.Red;
            await this._focuser.StepAsync(false, Convert.ToInt32(this.slStepCount.Value) * 2);
            this.btnLLeft.Foreground = Brushes.Black;
            this._setEnabledInputs(true);
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (this._isCapturing)
            {
                this._isCapturing = false;
                this.btnStart.IsEnabled = false;
                return;
            }

            this._isCapturing = true;
            this._setEnabledInputs(false);
            int pictureCount = Convert.ToInt32(this.txtPictureCount.Text);
            int seconds = Convert.ToInt32(this.txtTimeout.Text);
            int steps = Convert.ToInt32(this.slStepCount.Value);

            this.btnStart.Content = "Stop";

            await Task.Run(async () =>
            {

                for (int i = 0; i < pictureCount; i++)
                {
                    bool success = SonyWindowClicker.DoPhoto();
                    
                    this.lblInfo.Dispatcher.Invoke(() =>
                    {
                        lblInfo.Content = $"Images: {i + 1}";
                    });

                    await Task.Delay(seconds * 1000);

                    if(!this._isCapturing)
                    {
                        return;
                    }

                    if (!success)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("The Sony remote application is not available, please start it!", "Error");
                        });
                        break;
                    }

                    this._focuser.Step(true, steps);
                }
            });

            this._isCapturing = false;
            this.btnStart.IsEnabled = true;
            this.lblInfo.Content = "";
            this.btnStart.Content = "Start";
            this._setEnabledInputs(true);
        }

        private async void btnToStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this._setEnabledInputs(false);
                this.btnToStart.Foreground = Brushes.Red;
                int steps = Convert.ToInt32(this.slStepCount.Value);
                int pictures = Convert.ToInt32(this.txtPictureCount.Text);
                await this._focuser.StepAsync(false, steps * pictures);
            }
            catch (Exception)
            {
                MessageBox.Show("Picture count is not set!", "Error");
            }
            finally
            {
                this.btnToStart.Foreground = Brushes.Black;
                this._setEnabledInputs(true);
            }
        }

        private async void btnToEnd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this._setEnabledInputs(false);
                this.btnToEnd.Foreground = Brushes.Red;
                int steps = Convert.ToInt32(this.slStepCount.Value);
                int pictures = Convert.ToInt32(this.txtPictureCount.Text);
                await this._focuser.StepAsync(true, steps * pictures);
            }
            catch (Exception)
            {
                MessageBox.Show("Picture count is not set!", "Error");
            }
            finally
            {
                this.btnToEnd.Foreground = Brushes.Black;
                this._setEnabledInputs(true);
            }
        }

        private void cbAlwaysOnTop_Click(object sender, RoutedEventArgs e)
        {
            bool onTop = cbAlwaysOnTop.IsChecked.GetValueOrDefault();
            this.Topmost = onTop;
        }

        private void cbSerialportsDropDown(object sender, EventArgs e)
        {
            this._setSerialPorts();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            this.saveState();
        }

        /// <summary>
        /// Sets the default values for controls
        /// </summary>
        private void loadState()
        {
            Config.Init();
            this.cbAlwaysOnTop.IsChecked = Config.IsAlwaysOnTop;
            this.Topmost = Config.IsAlwaysOnTop;
            this.slStepCount.Value = Config.SliderValue;
            this.txtPictureCount.Text = Config.PictureCount.ToString();
            this.txtTimeout.Text = Config.TimeBetweenPictures.ToString();
            this.PresetA.Content = Config.Presets[0].Name;
            this.PresetB.Content = Config.Presets[1].Name;
            this.PresetC.Content = Config.Presets[2].Name;
            this.PresetD.Content = Config.Presets[3].Name;
        }

        /// <summary>
        /// Saves the state of the window to xml file
        /// </summary>
        private void saveState()
        {
            Config.IsAlwaysOnTop = this.cbAlwaysOnTop.IsChecked.GetValueOrDefault();
            Config.SliderValue = Convert.ToInt32(this.slStepCount.Value);
            Config.PictureCount = Convert.ToInt32(this.txtPictureCount.Text);
            Config.TimeBetweenPictures = Convert.ToInt32(this.txtTimeout.Text);
            Config.SaveConfig();
        }

        /// <summary>
        /// Set the steps sider value to a given preset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PresetBtn_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            int index = Convert.ToInt32(tag);
            if(Config.Presets.Count > index)
            {
                this.slStepCount.Value = Config.Presets[index].Steps;
            }
        }

        /// <summary>
        /// Modify the preset value and name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PresetBtn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            int index = Convert.ToInt32(tag);
            if (Config.Presets.Count > index)
            {
                var preset = Config.Presets[index];
                PresetDialogbox pdb = new PresetDialogbox(preset);
                pdb.Owner = this;

                if(pdb.ShowDialog() == true)
                {
                    preset.Name = pdb.Name;
                    preset.Steps = pdb.Steps;
                    (sender as Button).Content = preset.Name;
                }
            }
        }
    }
}
