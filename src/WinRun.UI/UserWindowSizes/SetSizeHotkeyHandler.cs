using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinRun.Hotkeys;
using WinRun.UserWindowSizes.UI;
using WinRun.UserWindowSizes.UI.ViewModels;

namespace WinRun.UserWindowSizes
{
    public class SetSizeHotkeyHandler : IHotkeyHandler
    {
        private SetSizeWindow window;

        public Hotkey Hotkey { get; private set; }

        public SetSizeHotkeyHandler()
        {
            Hotkey = new Hotkey(ModifierKeys.Windows | ModifierKeys.Shift, Key.A);
        }

        public void Handle(Hotkey hotkey)
        {
            if (window != null)
            {
                window.Close();
                window = null;
            }

            IntPtr activeWindowHandle = Win32.GetForegroundWindow();
            Win32.RECT activeWindow;
            if (Win32.GetWindowRect(activeWindowHandle, out activeWindow))
            {
                SetSizeViewModel viewModel = new SetSizeViewModel("Wnd", new WindowManager(activeWindowHandle));
                viewModel.Left = activeWindow.Left;
                viewModel.Top = activeWindow.Top;
                viewModel.Width = activeWindow.Right - activeWindow.Left;
                viewModel.Height = activeWindow.Bottom - activeWindow.Top;

                window = new SetSizeWindow();
                window.ViewModel = viewModel;
                window.Show();
            }
        }

        private class WindowManager : SetSizeViewModel.IWindowManager
        {
            private readonly IntPtr handle;

            public WindowManager(IntPtr handle)
            {
                Ensure.NotNull(handle, "handle");
                this.handle = handle;
            }

            public void Update(int left, int top, int width, int height)
            {
                Win32.SetWindowPos(handle, IntPtr.Zero, left, top, width, height, 0);
            }
        }
    }
}
