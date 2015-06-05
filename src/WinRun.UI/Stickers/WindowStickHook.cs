﻿using System;
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
                    
                    StickContext leftContext = new StickContext(info.Left);
                    StickContext topContext = new StickContext(info.Top);
                    int width = info.Right - info.Left;
                    int height = info.Bottom - info.Top;
                    int leftPriority = Int32.MaxValue;
                    int topPriority = Int32.MaxValue;

                    foreach (StickPoint other in pointProvider.ForLeft())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        if (leftPriority < other.Priority)
                            break;

                        if (leftContext.TryStickTo(other.Value))
                            leftPriority = other.Priority;
                    }

                    foreach (StickPoint other in pointProvider.ForRight())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        if (leftPriority < other.Priority)
                            break;

                        if (leftContext.TryStickTo(other.Value, width))
                            leftPriority = other.Priority;
                    }

                    foreach (StickPoint other in pointProvider.ForTop())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        if (topPriority < other.Priority)
                            break;

                        if (topContext.TryStickTo(other.Value))
                            topPriority = other.Priority;
                    }

                    foreach (StickPoint other in pointProvider.ForBottom())
                    {
                        if (other.Handle == hwnd)
                            continue;

                        if (topPriority < other.Priority)
                            break;

                        if (topContext.TryStickTo(other.Value, height))
                            topPriority = other.Priority;
                    }

                    Log("Final state: {0}x{1} at {2}x{3}.", leftContext.NewPosition, topContext.NewPosition, height, width);
                    Win32.SetWindowPos(hwnd, IntPtr.Zero, leftContext.NewPosition, topContext.NewPosition, width, height, 0);
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
