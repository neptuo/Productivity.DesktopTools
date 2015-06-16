using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using WinRun.Hotkeys;

namespace WinRun.UI
{
    public class TurnMonitorOffHandler : IHotkeyHandler
    {
        private readonly Window window;

        public TurnMonitorOffHandler(Window window)
        {
            this.window = window;
            Hotkey = new Hotkey(ModifierKeys.Windows, Key.F3);
        }

        public Hotkey Hotkey { get; private set; }

        public void Handle(Hotkey hotkey)
        {
            WindowInteropHelper interop = new WindowInteropHelper(window);
            Win32.SendMessage(interop.Handle, Win32.WM_SYSCOMMAND, Win32.SC_MONITORPOWER, Win32.MONITOR_OFF);
        }

        private static class Win32
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

            public static int WM_SYSCOMMAND = 0x112;
            public static IntPtr SC_MONITORPOWER = new IntPtr(0xF170);

            public static IntPtr MONITOR_ON = new IntPtr(-1);
            public static IntPtr MONITOR_OFF = new IntPtr(2);
            public static IntPtr MONITOR_STANBY = new IntPtr(1);
        }
    }
}
