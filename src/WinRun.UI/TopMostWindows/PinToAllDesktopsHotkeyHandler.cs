using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VirtualDesktops;
using WinRun.Hotkeys;

namespace WinRun.TopMostWindows
{
    public class PinToAllDesktopsHotkeyHandler : IHotkeyHandler
    {
        public Hotkey Hotkey => new Hotkey(ModifierKeys.Windows | ModifierKeys.Alt, Key.X);

        public void Handle(Hotkey hotkey)
        {
            IntPtr handle = Win32.GetForegroundWindow();
            if (Desktop.IsWindowPinned(handle))
                Desktop.UnpinWindow(handle);
            else
                Desktop.PinWindow(handle);
        }
    }
}
