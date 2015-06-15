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
using WinRun.UI.Hotkeys;
using WinRun.UI.Stickers;

namespace WinRun.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClockWindow clockWindow = null;
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

            hotkeyService.Add(ModifierKeys.Windows, Key.F3, delegate
            {
                WindowInteropHelper interop = new WindowInteropHelper(this);
                SendMessage(interop.Handle, WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_OFF);
            });
            hotkeyService.Add(ModifierKeys.Windows, Key.F4, delegate
            {
                System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Suspend, true, true);
            });
            hotkeyService.Add(ModifierKeys.Windows, Key.F12, delegate
            {
                System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Hibernate, true, true);
            });
            hotkeyService.Add(ModifierKeys.Windows, Key.F5, delegate
            {
                Process.Start(@"C:\Windows\explorer.exe", "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}");
            });
            hotkeyService.Add(ModifierKeys.Windows, Key.F6, delegate
            {
                if (clockWindow == null)
                {
                    clockWindow = new ClockWindow();
                    clockWindow.Closed += (sender, args) => { clockWindow = null; };
                }
                clockWindow.SetClockSize(ClockSize.Large);
                clockWindow.Show();
                clockWindow.Activate();
            });
            hotkeyService.Add(ModifierKeys.Windows | ModifierKeys.Shift, Key.F6, delegate
            {
                if (clockWindow == null)
                {
                    clockWindow = new ClockWindow();
                    clockWindow.Closed += (sender, args) => { clockWindow = null; };
                }
                clockWindow.SetClockSize(ClockSize.Medium);
                clockWindow.Show();
                clockWindow.Activate();
            });
            hotkeyService.Add(ModifierKeys.Windows | ModifierKeys.Shift, Key.L, delegate
            {
                LockWorkStation();
                WindowInteropHelper interop = new WindowInteropHelper(this);
                SendMessage(interop.Handle, WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_OFF);
            });

            stickService = new StickService(Dispatcher);
            stickService.Install();
        }

        private void OnWinEvent(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            //throw new NotImplementedException();
        }

        [DllImport("user32.dll")]
        public static extern void LockWorkStation();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        static int WM_SYSCOMMAND = 0x112;
        static IntPtr SC_MONITORPOWER = new IntPtr(0xF170);

        static IntPtr MONITOR_ON = new IntPtr(-1);
        static IntPtr MONITOR_OFF = new IntPtr(2);
        static IntPtr MONITOR_STANBY = new IntPtr(1);

        private void Window_Activated_1(object sender, EventArgs e)
        {
            Hide();
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            stickService.UnInstall();
            hotkeyService.UnInstall();
        }

        private void Log(string messageFormat, params object[] parameters)
        {
            //DispatcherHelper.Run(
            //    Dispatcher,
            //    () => tbxLog.Text = String.Format(messageFormat, parameters) + Environment.NewLine + tbxLog.Text
            //);
            Console.WriteLine(messageFormat, parameters);
        }
    }
}
