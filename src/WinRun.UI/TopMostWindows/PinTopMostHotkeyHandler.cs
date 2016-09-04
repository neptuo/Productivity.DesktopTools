using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinRun.Hotkeys;

namespace WinRun.TopMostWindows
{
    public class PinTopMostHotkeyHandler : IHotkeyHandler
    {
        public Hotkey Hotkey
        {
            get { return new Hotkey(ModifierKeys.Windows | ModifierKeys.Alt, Key.Z); }
        }

        public void Handle(Hotkey hotkey)
        {
            IntPtr handle = Win32.GetForegroundWindow();
            if (Win32.IsWindowTopMost(handle))
                Win32.SetWindowPos(handle, Win32.NoTopMost, 0, 0, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_NOMOVE);
            else
                Win32.SetWindowPos(handle, Win32.TopMost, 0, 0, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_NOMOVE);
        }
    }
}
