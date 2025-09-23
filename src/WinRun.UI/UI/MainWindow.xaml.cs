using Neptuo;
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
using WinRun.TopMostWindows;
using WinRun.UI.TimeMeasuring;
using WinRun.UserWindowSizes;

namespace WinRun.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotkeyService hotkeyService;
        private readonly INotificationService notifications;

        public ClockHandler ClockHandler { get; private set; }
        public StickService StickService { get; private set; }

        public MainWindow(INotificationService notifications)
        {
            Ensure.NotNull(notifications, "notifications");
            this.notifications = notifications;

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
            hotkeyService.Add(new PinTopMostHotkeyHandler());
            hotkeyService.Add(new PinToAllDesktopsHotkeyHandler());

            SystemSuspendHandler suspendHandler = new SystemSuspendHandler();
            hotkeyService.Add(suspendHandler.SleepHotkey, suspendHandler.Handle);
            hotkeyService.Add(suspendHandler.HibernateHotkey, suspendHandler.Handle);

            ClockHandler = new ClockHandler();
            hotkeyService.Add(ClockHandler.LargeHotkey, ClockHandler.Handle);
            hotkeyService.Add(ClockHandler.MediumHotkey, ClockHandler.Handle);

            // http://stackoverflow.com/questions/34139450/getwindowrect-returns-a-size-including-invisible-borders
            StickService = new StickService(Dispatcher);
            StickService.Install();

            hotkeyService.Add(new SetSizeHotkeyHandler());
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            StickService.UnInstall();
            hotkeyService.UnInstall();
        }
    }
}
