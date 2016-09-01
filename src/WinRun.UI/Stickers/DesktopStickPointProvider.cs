using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRun.Stickers
{
    public class DesktopStickPointProvider : IStickPointProvider
    {
        private readonly int priority;

        public DesktopStickPointProvider(int priority)
        {
            this.priority = priority;

            IntPtr desktopWindow = Win32.GetDesktopWindow();
            Win32.RECT frame;
            int size = Marshal.SizeOf(typeof(Win32.RECT));
            int res = Win32.DwmGetWindowAttribute(desktopWindow, (int)Win32.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out frame, size);
        }

        public IEnumerable<StickPoint> ForTop()
        {
            return Screen.AllScreens.Select(s => new StickPoint(s.WorkingArea.Top, priority));
        }

        public IEnumerable<StickPoint> ForBottom()
        {
            return Screen.AllScreens.Select(s => new StickPoint(s.WorkingArea.Bottom, priority));
        }

        public IEnumerable<StickPoint> ForLeft()
        {
            // http://stackoverflow.com/questions/34139450/getwindowrect-returns-a-size-including-invisible-borders
            return Screen.AllScreens.Select(s => new StickPoint(s.WorkingArea.Left + Window10OffsetDecorator.DesktopLeftOffset, priority));
        }

        public IEnumerable<StickPoint> ForRight()
        {
            return Screen.AllScreens.Select(s => new StickPoint(s.WorkingArea.Right, priority));
        }
    }
}
