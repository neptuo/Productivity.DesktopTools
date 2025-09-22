using Neptuo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VirtualDesktops;
using WinRun.Hotkeys;
using WinRun.TopMostWindows.UI;

namespace WinRun.TopMostWindows
{
    public class PinToAllDesktopsHotkeyHandler : IHotkeyHandler
    {
        private PinWindow window = new PinWindow();

        public Hotkey Hotkey => new Hotkey(ModifierKeys.Windows | ModifierKeys.Alt, Key.X);

        public void Handle(Hotkey hotkey)
        {
            IntPtr handle = Win32.GetForegroundWindow();
            if (Desktop.IsWindowPinned(handle))
            {
                Desktop.UnpinWindow(handle);
                _ = ShowIconAsync(handle, false);
            }
            else
            {
                Desktop.PinWindow(handle);
                _ = ShowIconAsync(handle, true);
            }
        }

        private async Task ShowIconAsync(IntPtr handle, bool isPinned)
        {
            const int notificationDuration = 1 * 1000;

            if (Win32.GetWindowRect(handle, out var rect))
            {
                double windowHeight = window.ActualHeight;
                if (windowHeight == 0)
                    windowHeight = 96;

                double windowWidth = window.ActualWidth;
                if (windowWidth == 0)
                    windowWidth = 96;

                double top = rect.Top + ((rect.Bottom - rect.Top) / 2) - windowHeight / 2;
                double left = rect.Left + ((rect.Right - rect.Left) / 2) - windowWidth / 2;

                window.Top = top;
                window.Left = left;
                window.Show(isPinned);

                await Task.Delay(notificationDuration);

                window.Hide();
            }
        }
    }
}
