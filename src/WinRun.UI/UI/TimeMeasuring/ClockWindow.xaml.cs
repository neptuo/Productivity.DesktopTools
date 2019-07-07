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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using VirtualDesktops;
using WinRun.Properties;

namespace WinRun.UI.TimeMeasuring
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
        private IntPtr handle;
        private bool isActivated;

        public ClockWindow()
        {
            InitializeComponent();

            Settings.Default.IsClockOpen = true;
            Settings.Default.Save();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            timer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.Normal, OnTimerElapsed, Dispatcher);
            timer.Start();

            Left = Settings.Default.ClockLeft;
            Top = Settings.Default.ClockTop;
            UpdateDateTime();

            handle = new WindowInteropHelper(this).Handle;
            Win32.RemoveFromAeroPeek(handle);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (!isActivated)
            {
                Desktop.PinWindow(handle);
                isActivated = true;
            }
        }

        private void OnTimerElapsed(object sender, EventArgs e)
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

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Hide();
                UpdateSettings(false);

                Settings.Default.IsClockOpen = false;
                Settings.Default.Save();
            }
            else if (e.Key == Key.System && e.SystemKey == Key.F4)
            {
                e.Handled = true;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            timer.Stop();
            UpdateSettings();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void UpdateSettings(bool save = true)
        {
            Settings.Default.ClockLeft = Left;
            Settings.Default.ClockTop = Top;

            if (save)
                Settings.Default.Save();
        }
    }
}
