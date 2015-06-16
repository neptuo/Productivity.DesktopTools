using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WinRun.Properties;

namespace WinRun.UI
{
    public enum ClockSize
    {
        Large, Medium
    }

    /// <summary>
    /// Interaction logic for ClockWindow.xaml
    /// </summary>
    public partial class ClockWindow : Window
    {
        private DispatcherTimer timer;
        
        public ClockWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            timer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.Normal, Timer_Elapsed, Dispatcher);
            timer.Start();

            Left = Settings.Default.ClockLeft;
            Top = Settings.Default.ClockTop;
            UpdateDateTime();
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        public void UpdateDateTime()
        {
            lblClock.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        public void SetClockSize(ClockSize clockSize)
        {
            switch (clockSize)
            {
                case ClockSize.Large:
                    lblClock.FontSize = 80;
                    lblClock.Padding = new Thickness(30, 10, 30, 20);
                    break;
                case ClockSize.Medium:
                    lblClock.FontSize = 30;
                    lblClock.Padding = new Thickness(10, 10, 10, 12);
                    break;
                default:
                    break;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Hide();
                UpdateSettings();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            timer.Stop();
            UpdateSettings();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            //Hide();
            //UpdateSettings();
        }

        private void UpdateSettings()
        {
            Settings.Default.ClockLeft = Left;
            Settings.Default.ClockTop = Top;
            Settings.Default.Save();
        }
    }
}
