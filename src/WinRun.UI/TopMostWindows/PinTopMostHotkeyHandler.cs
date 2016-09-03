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
            get { return new Hotkey(ModifierKeys.Windows | ModifierKeys.Shift, Key.T); }
        }

        public void Handle(Hotkey hotkey)
        {
            IntPtr activeWindowHandle = Win32.GetForegroundWindow();
            // http://www.pinvoke.net/default.aspx/user32.getwindowlong
            // TODO: SetWindowPos with Win32.TopMost;
        }
    }
}
