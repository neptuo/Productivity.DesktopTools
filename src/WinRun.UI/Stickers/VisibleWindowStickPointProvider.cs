using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.Stickers
{
    public class VisibleWindowStickPointProvider : IStickPointProvider
    {
        private static string[] forbidden = new[]
        {
            "shell_traywnd",
            "progman"
            //"gesturefeedbackanimationwindow",
            //"hwndwrapper",
            //"edgeuiinputtopwndclass",
            //"internet explorer_hidden",
            //"desktopwallpapermanager",
            //"cabinetwclass",
            //"dummydwmlistenerwindow",
            //"workerw"
        };

        private readonly VirtualDesktopManager virtualDesktopManager = new VirtualDesktopManager();
        private readonly int priority;

        public VisibleWindowStickPointProvider(int priority)
        {
            this.priority = priority;
        }

        public IEnumerable<IntPtr> GetTopLevelWindows()
        {
            IEnumerable<IntPtr> allWindows = Win32.GetTopLevelWindows();
            IEnumerable<IntPtr> visibleWindows = allWindows.Where(Win32.IsWindowVisible).Where(Win32.IsWindow);

            Console.WriteLine("AllWindows: {0}; VisibleWindows: {1}", allWindows.Count(), visibleWindows.Count());
            foreach (IntPtr handle in visibleWindows)
            {
                if (!virtualDesktopManager.IsWindowOnCurrentVirtualDesktop(handle))
                    continue;

                StringBuilder className = new StringBuilder(1024);
                Win32.GetClassName(handle, className, className.Capacity);
                string classNameLower = className.ToString().ToLowerInvariant();

                StringBuilder text = new StringBuilder(1024);
                Win32.GetWindowText(handle, text, text.Capacity);
                string textLower = text.ToString().ToLowerInvariant();

                bool isSkipped = forbidden.Any(f => classNameLower.StartsWith(f));
                if (isSkipped)
                    continue;

                if (String.IsNullOrWhiteSpace(textLower))
                    continue;

                //Console.Write("TopLevelWindow: '{0}' -> '{1}'", className, text.ToString());

                //Win32.RECT info;
                //if (Win32.GetWindowRect(handle, out info))
                //    Console.Write(" :: {0}x{1} -> {2}x{3}", info.Top, info.Left, info.Bottom, info.Right);

                //Console.WriteLine();

                yield return handle;
            }
        }

        public IEnumerable<StickPoint> ForTop()
        {
            List<StickPoint> result = new List<StickPoint>();
            foreach (IntPtr handle in GetTopLevelWindows())
            {
                Win32.RECT info;
                Win32.DwmGetWindowAttribute(handle, (int)Win32.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out info, Marshal.SizeOf(typeof(Win32.RECT)));
                result.Add(new StickPoint(handle, info.Bottom, priority));
            }

            return result;
        }

        public IEnumerable<StickPoint> ForBottom()
        {
            List<StickPoint> result = new List<StickPoint>();
            foreach (IntPtr handle in GetTopLevelWindows())
            {
                Win32.RECT info;
                if (Win32.GetWindowRect(handle, out info))
                    result.Add(new StickPoint(handle, info.Top, priority));
            }

            return result;
        }

        public IEnumerable<StickPoint> ForLeft()
        {
            List<StickPoint> result = new List<StickPoint>();
            foreach (IntPtr handle in GetTopLevelWindows())
            {
                Win32.RECT info;
                Win32.DwmGetWindowAttribute(handle, (int)Win32.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out info, Marshal.SizeOf(typeof(Win32.RECT)));
                result.Add(new StickPoint(handle, info.Right + Window10OffsetDecorator.WindowOtherLeftOverlap, priority));
            }

            return result;
        }

        public IEnumerable<StickPoint> ForRight()
        {
            List<StickPoint> result = new List<StickPoint>();
            foreach (IntPtr handle in GetTopLevelWindows())
            {
                Win32.RECT info;
                Win32.DwmGetWindowAttribute(handle, (int)Win32.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out info, Marshal.SizeOf(typeof(Win32.RECT)));
                result.Add(new StickPoint(handle, info.Left, priority));
            }

            return result;
        }
    }
}
