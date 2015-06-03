using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRun.UI.Stickers
{
    public class WindowBoundsHook
    {
        private readonly IntPtr handle;
        private readonly Action<string, object[]> log;
        private readonly Win32.WinEventDelegate hookDelegate;

        public WindowBoundsHook(IntPtr handle, Action<string, object[]> log)
        {
            this.handle = handle;
            this.log = log;
            this.hookDelegate = new Win32.WinEventDelegate(WndProc2);
        }

        public WindowBoundsHook Install()
        {
            var result = Win32.SetWinEventHook(
                (uint)Win32.EventContants.EVENT_SYSTEM_MOVESIZESTART,
                (uint)Win32.EventContants.EVENT_SYSTEM_MOVESIZEEND,
                IntPtr.Zero,
                hookDelegate,
                0,
                0,
                (uint)Win32.SetWinEventHookParameter.WINEVENT_OUTOFCONTEXT
            );

            Log("SetWinEventHook on '{0}' returned '{1}'.", handle, result);
            return this;
        }

        public WindowBoundsHook Uninstall()
        {
            return this;
        }

        private void WndProc2(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (hwnd != handle)
                return;

            if (eventType == (uint)Win32.EventContants.EVENT_OBJECT_LOCATIONCHANGE)
            {
                Log("Location of '{0}' changed.", hwnd);
            }

            // Handle messages...
            if (eventType == (uint)Win32.EventContants.EVENT_SYSTEM_MOVESIZESTART)
            {
                Log("Starting to move or resize window '{0}' ('{1}', '{2}', '{3}', '{4}').", hwnd, idObject, idChild, dwEventThread, dwmsEventTime);
            }
            // Handle messages...
            if (eventType == (uint)Win32.EventContants.EVENT_SYSTEM_MOVESIZEEND)
            {
                Log("Resized or moved window '{0}' (from '{5}') ('{1}', '{2}', '{3}', '{4}').", hwnd, idObject, idChild, dwEventThread, dwmsEventTime, handle);
                //Win32.SetWindowPos(hwnd, IntPtr.Zero, 100, 100, 400, 400, 0);

                Win32.RECT info;
                if (Win32.GetWindowRect(hwnd, out info))
                {
                    Log("User state: {0}x{1} at {2}x{3}.", info.Top, info.Left, info.Bottom, info.Right);

                    int left = info.Left;
                    int top = info.Top;
                    int width = info.Right - info.Left;
                    int height = info.Bottom - info.Top;

                    int newLeft = left;
                    int newTop = top;

                    //if (info.Top < 10)
                    //    info.Top = 0;
                    //else if (info.Bottom > screenHeight - 10)
                    //    info.Top = screenHeight - height;

                    //if (info.Right > screenWidth - 10)
                    //    info.Left = screenWidth - width;

                    int all = WindowList.GetAllWindows().Select(w => w.Handle).Count();
                    int diff = WindowList.GetAllWindows().Select(w => w.Handle).Where(h => h != hwnd).Count() - all;

                    bool leftModified = false;
                    bool topModified = false;

                    foreach (StickInfo other in GetLeftStickHolders())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        if (other.Value - MainWindow.StickOffset < left && left < other.Value + MainWindow.StickOffset)
                        {
                            if (!leftModified || Math.Abs(newLeft - left) > Math.Abs(other.Value - left))
                            {
                                newLeft = Math.Max(other.Value, 0);
                                leftModified = true;
                                Log("Posible stick left '{0}' ({1}).", other.Handle, other.Value);
                            }
                        }
                    }

                    foreach (StickInfo other in GetTopStickHolders())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        if (other.Value - MainWindow.StickOffset < top && top < other.Value + MainWindow.StickOffset)
                        {
                            if (!topModified || Math.Abs(newTop - top) > Math.Abs(other.Value - top))
                            {
                                newTop = Math.Max(other.Value, 0);
                                topModified = true;
                                Log("Posible stick top '{0}' ({1}).", other.Handle, other.Value);
                            }
                        }
                    }

                    foreach (WindowInfo other in GetStickHolders())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        //if (other.Left - MainWindow.StickOffset < left && left < other.Left + MainWindow.StickOffset)
                        //{
                        //    if (!leftModified || Math.Abs(newLeft - left) > Math.Abs(other.Left - left))
                        //    {
                        //        newLeft = Math.Max(other.Left, 0);
                        //        leftModified = true;
                        //        Log("Posible stick left '{0}' ({1}x{2}->{3}x{4}).", other.Handle, other.Left, other.Top, other.Width, other.Height);
                        //    }
                        //}

                        if (other.Top - MainWindow.StickOffset < top && top < other.Top + MainWindow.StickOffset)
                        {
                            if (!topModified || Math.Abs(newTop - top) > Math.Abs(other.Top - top))
                            {
                                newTop = Math.Max(other.Top, 0);
                                topModified = true;
                                Log("Posible stick top '{0}' ({1}x{2}->{3}x{4}).", other.Handle, other.Left, other.Top, other.Width, other.Height);
                            }
                        }
                    }


                    //StringBuilder output = new StringBuilder();
                    //foreach (IntPtr x in GetStickHolders().Select(w => w.Handle))
                    //{
                    //    StringBuilder className = new StringBuilder(1024);
                    //    StringBuilder caption = new StringBuilder(1024);
                    //    Win32.GetWindowText(x, caption, caption.Capacity);
                    //    Win32.GetClassName(x, className, className.Capacity);

                    //    output.AppendLine(caption.ToString());
                    //    output.AppendLine(className.ToString());
                    //    output.AppendLine();
                    //}

                    //System.IO.File.WriteAllText("C:/Temp/Windows.txt", output.ToString());





                    //foreach (Screen screen in Screen.AllScreens)
                    //{
                    //    if (screen.WorkingArea.Left < left && left < screen.WorkingArea.Left + screen.WorkingArea.Width)
                    //    {
                    //        // This is the screen.
                    //        if (!leftModified)
                    //        {
                    //            if (left < screen.WorkingArea.Left + MainWindow.StickOffset)
                    //                newLeft = screen.WorkingArea.Left;
                    //            else if (left + width > screen.WorkingArea.Left + screen.WorkingArea.Width - MainWindow.StickOffset)
                    //                newLeft = screen.WorkingArea.Left + screen.WorkingArea.Width - width;
                    //        }

                    //        if (!topModified)
                    //        {
                    //            if (top < screen.WorkingArea.Top + MainWindow.StickOffset)
                    //                newTop = screen.WorkingArea.Top;
                    //            else if (top + height > screen.WorkingArea.Top + screen.WorkingArea.Height - MainWindow.StickOffset)
                    //                newTop = screen.WorkingArea.Top + screen.WorkingArea.Height - height;
                    //        }
                    //    }
                    //}

                    Log("Final state: {0}x{1} at {2}x{3}.", newTop, newLeft, height, width);
                    Win32.SetWindowPos(hwnd, IntPtr.Zero, newLeft, newTop, width, height, 0);
                }
                else
                {
                    Log("Unnable to get window info '{0}'.", hwnd);
                }
            }
        }

        private string GetWindowText(IntPtr? handle)
        {
            if (handle == null)
                return "NO HANDLE";

            StringBuilder content = new StringBuilder(1024);
            Win32.GetWindowText(handle.Value, content, content.Capacity);
            return content.ToString();
        }

        private void Log(string messageFormat, params object[] parameters)
        {
            log(messageFormat, parameters);
        }

        private IEnumerable<StickInfo> GetLeftStickHolders()
        {
            return Enumerable.Concat(
                Screen.AllScreens.Select(s => new StickInfo(s.WorkingArea.Left)),
                WindowList.GetTopLevelWindows()
                    .Where(Win32.IsWindowVisible)
                    .Select(GetWindowInfoOrDefalt)
                    .Where(i => i != null)
                    .Select(i => new StickInfo(i.Handle, i.Left + i.Width))
            );
        }

        private IEnumerable<StickInfo> GetTopStickHolders()
        {
            return Enumerable.Concat(
                Screen.AllScreens.Select(s => new StickInfo(s.WorkingArea.Top)),
                WindowList.GetTopLevelWindows()
                    .Where(Win32.IsWindowVisible)
                    .Select(GetWindowInfoOrDefalt)
                    .Where(i => i != null)
                    .Select(i => new StickInfo(i.Handle, i.Top + i.Height))
            );
        }

        private IEnumerable<WindowInfo> GetStickHolders()
        {
            return Enumerable.Concat(
                Screen.AllScreens.Select(s => new WindowInfo(IntPtr.Zero, s.WorkingArea.Left, s.WorkingArea.Top, s.WorkingArea.Width, s.WorkingArea.Height)),
                WindowList.GetTopLevelWindows()
                    .Where(Win32.IsWindowVisible)
                    .Select(GetWindowInfoOrDefalt)
                    .Where(i => i != null)
            );
        }

        private WindowInfo GetWindowInfoOrDefalt(IntPtr handle)
        {
            Win32.RECT info;
            if (Win32.GetWindowRect(handle, out info))
                return new WindowInfo(handle, info.Left, info.Top, info.Right - info.Left, info.Bottom - info.Top);

            return null;
        }

        //private IEnumerable<int> GetTopStickHolders(IntPtr hwnd)
        //{
        //    return Enumerable.Concat(
        //        Screen.AllScreens.Select(s => s.WorkingArea.Top),
        //        WindowList.GetTopLevelWindows().Where(h => h != hwnd).Select(h =>
        //        {
        //            Win32.RECT info;
        //            if (Win32.GetWindowRect(h, out info))
        //                return info.Bottom;

        //            return 0;
        //        })
        //    );
        //}

        private class StickInfo
        {
            public IntPtr? Handle;
            public int Value;

            public StickInfo(int value)
            {
                Value = value;
            }

            public StickInfo(IntPtr handle, int value)
                : this(value)
            {
                Handle = handle;
            }
        }

        private class WindowInfo
        {
            public IntPtr Handle;
            public int Left;
            public int Top;
            public int Width;
            public int Height;

            public WindowInfo(IntPtr handle, int left, int top, int width, int height)
            {
                Handle = handle;
                Left = left;
                Top = top;
                Width = width;
                Height = height;
            }
        }
    }
}
