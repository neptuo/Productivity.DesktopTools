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

namespace WinRun.TopMostWindows.UI
{
    public partial class PinWindow : Window
    {
        public PinWindow() 
            => InitializeComponent();

        public void Show(bool isPinned)
        {
            Icon.Foreground = new SolidColorBrush(isPinned ? Colors.LightGreen : Colors.Red);
            Show();
        }
    }
}
