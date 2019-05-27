using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.ShellHooks
{
    internal static class Win32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCT
        {
            public POINT pt; // Can't use System.Windows.Point because that has X,Y as doubles, not integer
            public IntPtr hwnd;
            public uint wHitTestCode;
            public IntPtr dwExtraInfo;
            public override string ToString()
            {
                return $"({pt.X,4},{pt.Y,4})";
            }
        }

#pragma warning disable 649 // CS0649: Field 'MainWindow.NativeMethods.POINT.Y' is never assigned to, and will always have its default value 0
        public struct POINT
        {
            public int X;
            public int Y;
        }
#pragma warning restore 649

        // from WinUser.h
        public enum HookType
        {
            WH_MIN = (-1),
            WH_MSGFILTER = (-1),
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }
        public enum HookCodes
        {
            HC_ACTION = 0,
            HC_GETNEXT = 1,
            HC_SKIP = 2,
            HC_NOREMOVE = 3,
            HC_NOREM = HC_NOREMOVE,
            HC_SYSMODALON = 4,
            HC_SYSMODALOFF = 5
        }
        public enum CBTHookCodes
        {
            HCBT_MOVESIZE = 0,
            HCBT_MINMAX = 1,
            HCBT_QS = 2,
            HCBT_CREATEWND = 3,
            HCBT_DESTROYWND = 4,
            HCBT_ACTIVATE = 5,
            HCBT_CLICKSKIPPED = 6,
            HCBT_KEYSKIPPED = 7,
            HCBT_SYSCOMMAND = 8,
            HCBT_SETFOCUS = 9
        }

        public delegate IntPtr CBTProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hookPtr);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hookPtr, int nCode, IntPtr wordParam, IntPtr longParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(HookType hookType, CBTProc hookProc, IntPtr instancePtr, uint threadID);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookExA(HookType hookType, CBTProc hookProc, IntPtr instancePtr, uint threadID);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }
}
