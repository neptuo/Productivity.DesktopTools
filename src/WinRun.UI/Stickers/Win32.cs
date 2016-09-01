using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinRun.Stickers
{
    internal static class Win32
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        #region Solution 1

        [DllImport("user32.dll", EntryPoint = "SetWinEventHook", SetLastError = true)]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hook);

        // Callback function
        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        // Enums
        public enum EventContants : uint
        {
            EVENT_SYSTEM_SOUND = 0x1,
            EVENT_SYSTEM_ALERT = 0x2,
            EVENT_SYSTEM_FOREGROUND = 0x3,
            EVENT_SYSTEM_MENUSTART = 0x4,
            EVENT_SYSTEM_MENUEND = 0x5,
            EVENT_SYSTEM_MENUPOPUPSTART = 0x6,
            EVENT_SYSTEM_MENUPOPUPEND = 0x7,
            EVENT_SYSTEM_CAPTURESTART = 0x8,
            EVENT_SYSTEM_CAPTUREEND = 0x9,
            EVENT_SYSTEM_MOVESIZESTART = 0xa,
            EVENT_SYSTEM_MOVESIZEEND = 0xb,
            EVENT_SYSTEM_CONTEXTHELPSTART = 0xc,
            EVENT_SYSTEM_CONTEXTHELPEND = 0xd,
            EVENT_SYSTEM_DRAGDROPSTART = 0xe,
            EVENT_SYSTEM_DRAGDROPEND = 0xf,
            EVENT_SYSTEM_DIALOGSTART = 0x10,
            EVENT_SYSTEM_DIALOGEND = 0x11,
            EVENT_SYSTEM_SCROLLINGSTART = 0x12,
            EVENT_SYSTEM_SCROLLINGEND = 0x13,
            EVENT_SYSTEM_SWITCHSTART = 0x14,
            EVENT_SYSTEM_SWITCHEND = 0x15,
            EVENT_SYSTEM_MINIMIZESTART = 0x16,
            EVENT_SYSTEM_MINIMIZEEND = 0x17,

            EVENT_OBJECT_LOCATIONCHANGE = 0x800B
        }

        [Flags]
        internal enum SetWinEventHookParameter
        {
            WINEVENT_OUTOFCONTEXT = 0,
            WINEVENT_SKIPOWNTHREAD = 1,
            WINEVENT_SKIPOWNPROCESS = 2,
            WINEVENT_INCONTEXT = 4
        }

        #endregion

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, ref RECT pvAttribute, int cbAttribute);

        [Flags]
        public enum DwmWindowAttribute : uint
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }


        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int W, int H, uint uFlags);

        public static class SWP
        {
            public static readonly int

            NOSIZE = 0x0001,

            NOMOVE = 0x0002,

            NOZORDER = 0x0004,

            NOREDRAW = 0x0008,

            NOACTIVATE = 0x0010,

            DRAWFRAME = 0x0020,

            FRAMECHANGED = 0x0020,

            SHOWWINDOW = 0x0040,

            HIDEWINDOW = 0x0080,

            NOCOPYBITS = 0x0100,

            NOOWNERZORDER = 0x0200,

            NOREPOSITION = 0x0200,

            NOSENDCHANGING = 0x0400,

            DEFERERASE = 0x2000,

            ASYNCWINDOWPOS = 0x4000;

        }


        private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowProc callback, IntPtr i);

        public static List<IntPtr> GetTopLevelWindows()
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumWindows(childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");

            list.Add(handle);
            return true;
        }


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }




        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
        [SuppressUnmanagedCodeSecurity]
        public interface IVirtualDesktopManager
        {
            [PreserveSig]
            int IsWindowOnCurrentVirtualDesktop(
                [In] IntPtr TopLevelWindow,
                [Out] out int OnCurrentDesktop
                );
            [PreserveSig]
            int GetWindowDesktopId(
                [In] IntPtr TopLevelWindow,
                [Out] out Guid CurrentDesktop
                );

            [PreserveSig]
            int MoveWindowToDesktop(
                [In] IntPtr TopLevelWindow,
                [MarshalAs(UnmanagedType.LPStruct)]
            [In]Guid CurrentDesktop
                );
        }

        [ComImport, Guid("aa509086-5ca9-4c25-8f95-589d3c07b48a")]
        public class CVirtualDesktopManager
        {

        }
    }

    public class VirtualDesktopManager
    {
        public VirtualDesktopManager()
        {
            cmanager = new Win32.CVirtualDesktopManager();
            manager = (Win32.IVirtualDesktopManager)cmanager;
        }

        ~VirtualDesktopManager()
        {
            manager = null;
            cmanager = null;
        }

        private Win32.CVirtualDesktopManager cmanager = null;
        private Win32.IVirtualDesktopManager manager;

        public bool IsWindowOnCurrentVirtualDesktop(IntPtr TopLevelWindow)
        {
            int result;
            int hr;
            if ((hr = manager.IsWindowOnCurrentVirtualDesktop(TopLevelWindow, out result)) != 0)
            {
                Exception ex = Marshal.GetExceptionForHR(hr);
                if (ex != null)
                    return false;
            }
            return result != 0;
        }

        public Guid GetWindowDesktopId(IntPtr TopLevelWindow)
        {
            Guid result;
            int hr;
            if ((hr = manager.GetWindowDesktopId(TopLevelWindow, out result)) != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
            return result;
        }

        public void MoveWindowToDesktop(IntPtr TopLevelWindow, Guid CurrentDesktop)
        {
            int hr;
            if ((hr = manager.MoveWindowToDesktop(TopLevelWindow, CurrentDesktop)) != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }
    }
}
