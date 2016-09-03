using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.TopMostWindows
{
    internal static class Win32
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public static IntPtr TopMost = new IntPtr(-1);
        public static IntPtr NoTopMost = new IntPtr(-2);
    }
}
