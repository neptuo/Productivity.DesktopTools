using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
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

            IntPtr activeWindowHwnd = Win32.GetForegroundWindow();
            Win32.RECT activeWindow;
            if (Win32.GetWindowRect(activeWindowHwnd, out activeWindow))
            {
                StringBuilder text = new StringBuilder(1024);
                Win32.GetWindowText(activeWindowHwnd, text, text.Capacity);

                window = new SetSizeWindow();

                WindowManager windowManager = new(activeWindowHwnd, IntPtr.Zero);
                SetSizeViewModel viewModel = new SetSizeViewModel(text.ToString(), windowManager);
                viewModel.Left = activeWindow.Left;
                viewModel.Top = activeWindow.Top;
                viewModel.Width = activeWindow.Right - activeWindow.Left;
                viewModel.Height = activeWindow.Bottom - activeWindow.Top;

                window.ViewModel = viewModel;
                window.Show();

                var setSizeWindowHandle = new WindowInteropHelper(window).Handle;
                windowManager.ToFocusHwnd = setSizeWindowHandle;
            }
        }

        private class WindowManager(IntPtr toMoveHwnd, IntPtr toFocusHwnd) : SetSizeViewModel.IWindowManager
        {
            public IntPtr ToFocusHwnd { get; set; } = toFocusHwnd;

            public void Update(int left, int top, int width, int height, bool isCurrentMonitor)
            {
                Win32.RECT frame;
                Win32.DwmGetWindowAttribute(toMoveHwnd, (int)Win32.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out frame, Marshal.SizeOf(typeof(Win32.RECT)));

                Win32.RECT position;
                if (Win32.GetWindowRect(toMoveHwnd, out position))
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
                            toMoveHwnd,
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
                            toMoveHwnd, 
                            IntPtr.Zero, 
                            left + (position.Left - frame.Left),
                            top + (position.Top - frame.Top), 
                            width, 
                            height, 
                            0
                        );
                    }
                }

                if (ToFocusHwnd != IntPtr.Zero)
                    Win32.SetForegroundWindow(ToFocusHwnd);
            }
        }
    }
}
