using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRun.UI.Stickers
{
    public class WindowStickHook : IBackgroundService
    {
        private readonly IntPtr handle;
        private readonly IStickPointProvider pointProvider;
        private readonly Win32.WinEventDelegate hookDelegate;
        private IntPtr hookPointer;

        public WindowStickHook(IntPtr handle, IStickPointProvider pointProvider)
        {
            this.handle = handle;
            this.pointProvider = pointProvider;
            this.hookDelegate = new Win32.WinEventDelegate(WndProc2);
        }

        public void Install()
        {
            hookPointer = Win32.SetWinEventHook(
                (uint)Win32.EventContants.EVENT_SYSTEM_MOVESIZESTART,
                (uint)Win32.EventContants.EVENT_SYSTEM_MOVESIZEEND,
                IntPtr.Zero,
                hookDelegate,
                0,
                0,
                (uint)Win32.SetWinEventHookParameter.WINEVENT_OUTOFCONTEXT
            );

            Log("SetWinEventHook on '{0}' returned '{1}'.", handle, hookPointer);
        }

        public void UnInstall()
        {
            Win32.UnhookWinEvent(hookPointer);
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

                    foreach (StickInfo other in pointProvider.ForLeft())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        if (other.Value - StickService.StickOffset < left && left < other.Value + StickService.StickOffset)
                        {
                            if (!leftModified || Math.Abs(newLeft - left) > Math.Abs(other.Value - left))
                            {
                                newLeft = Math.Max(other.Value, 0);
                                leftModified = true;
                                Log("Posible stick left '{0}' ({1}).", other.Handle, other.Value);
                            }
                        }
                    }

                    foreach (StickInfo other in pointProvider.ForRight())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        if (other.Value - StickService.StickOffset < left + width && left + width < other.Value + StickService.StickOffset)
                        {
                            if (!leftModified || Math.Abs(newLeft - left) > Math.Abs(other.Value - (left + width)))
                            {
                                newLeft = Math.Max(other.Value - width, 0);
                                leftModified = true;
                                Log("Posible stick right '{0}' ({1}).", other.Handle, other.Value);
                            }
                        }
                    }

                    foreach (StickInfo other in pointProvider.ForTop())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        if (other.Value - StickService.StickOffset < top && top < other.Value + StickService.StickOffset)
                        {
                            if (!topModified || Math.Abs(newTop - top) > Math.Abs(other.Value - top))
                            {
                                newTop = Math.Max(other.Value, 0);
                                topModified = true;
                                Log("Posible stick top '{0}' ({1}).", other.Handle, other.Value);
                            }
                        }
                    }

                    foreach (StickInfo other in pointProvider.ForBottom())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        if (other.Value - StickService.StickOffset < top + height && top + height < other.Value + StickService.StickOffset)
                        {
                            if (!topModified || Math.Abs(newTop - top) > Math.Abs(other.Value - (top + height))) 
                            {
                                newTop = Math.Max(other.Value - height, 0);
                                topModified = true;
                                Log("Posible stick bottom '{0}' ({1}).", other.Handle, other.Value);
                            }
                        }
                    }

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
        }
    }
}
