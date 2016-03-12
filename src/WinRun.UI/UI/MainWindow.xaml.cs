using Neptuo.Windows.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinRun.Hotkeys;
using WinRun.Stickers;
using WinRun.UI.TimeMeasuring;
using WinRun.UserWindowSizes;

namespace WinRun.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StickService stickService;
        private HotkeyService hotkeyService;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            hotkeyService = new HotkeyService(this);
            hotkeyService.Install();

            hotkeyService.Add(new TurnMonitorOffHandler(this));
            hotkeyService.Add(new NetworkConnectionsHandler());
            hotkeyService.Add(new LockWorkStationHandler(this));

            SystemSuspendHandler suspendHandler = new SystemSuspendHandler();
            hotkeyService.Add(suspendHandler.SleepHotkey, suspendHandler.Handle);
            hotkeyService.Add(suspendHandler.HibernateHotkey, suspendHandler.Handle);

            ClockHandler clockHandler = new ClockHandler();
            hotkeyService.Add(clockHandler.LargeHotkey, clockHandler.Handle);
            hotkeyService.Add(clockHandler.MediumHotkey, clockHandler.Handle);

            stickService = new StickService(Dispatcher);
            stickService.Install();

            hotkeyService.Add(new SetSizeHotkeyHandler());
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            stickService.UnInstall();
            hotkeyService.UnInstall();
        }
    }
}
