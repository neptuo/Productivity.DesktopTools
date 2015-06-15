using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using WinRun.UI.Hotkeys;

namespace WinRun.UI
{
    public class LockWorkStationHandler : TurnMonitorOffHandler, IHotkeyHandler
    {
        public new Hotkey Hotkey { get; private set; }

        public LockWorkStationHandler(Window window)
            : base(window)
        {
            Hotkey = new Hotkey(ModifierKeys.Windows | ModifierKeys.Shift, Key.L);
        }

        public new void Handle(Hotkey hotkey)
        {
            Win32.LockWorkStation();
            base.Handle(hotkey);
        }

        private static class Win32
        {
            [DllImport("user32.dll")]
            public static extern void LockWorkStation();
        }
    }
}
