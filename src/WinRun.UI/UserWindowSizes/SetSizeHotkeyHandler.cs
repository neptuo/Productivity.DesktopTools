using Neptuo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            Hotkey = new Hotkey(ModifierKeys.Windows | ModifierKeys.Alt, Key.A);
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

            public void Update(int left, int top, int width, int height, bool isCurrentMonitor)
            {
                Win32.RECT frame;
                Win32.DwmGetWindowAttribute(handle, (int)Win32.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out frame, Marshal.SizeOf(typeof(Win32.RECT)));

                Win32.RECT position;
                if (Win32.GetWindowRect(handle, out position))
                {
                    if (isCurrentMonitor)
                    {
                        Screen screen = Screen.FromRectangle(new Rectangle(
                            position.Left,
                            position.Top,
                            position.Right - position.Left,
                            position.Bottom - position.Top
                        ));

                        Win32.SetWindowPos(
                            handle,
                            IntPtr.Zero,
                            left + screen.WorkingArea.Left + (position.Left - frame.Left),
                            top + screen.WorkingArea.Top + (position.Top - frame.Top),
                            width,
                            height,
                            0
                        );
                    }
                    else
                    {
                        Win32.SetWindowPos(
                            handle, 
                            IntPtr.Zero, 
                            left + (position.Left - frame.Left),
                            top + (position.Top - frame.Top), 
                            width, 
                            height, 
                            0
                        );
                    }
                }
            }
        }
    }
}
