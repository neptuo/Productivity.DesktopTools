using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.Stickers
{
    public class VisibleWindowStickPointProvider : IStickPointProvider
    {
        private readonly int priority;

        public VisibleWindowStickPointProvider(int priority)
        {
            this.priority = priority;
        }

        private IEnumerable<IntPtr> GetTopLevelWindows()
        {
            string[] forbidden = new[] { "shell_traywnd", "progman" };

            foreach (IntPtr handle in Win32.GetTopLevelWindows().Where(Win32.IsWindowVisible))
            {
                StringBuilder className = new StringBuilder();
                Win32.GetClassName(handle, className, className.Capacity);

                if (forbidden.Contains(className.ToString()))
                    continue;

                yield return handle;
            }
        }

        public IEnumerable<StickPoint> ForTop()
        {
            List<StickPoint> result = new List<StickPoint>();
            foreach (IntPtr handle in GetTopLevelWindows())
            {
                Win32.RECT info;
                if (Win32.GetWindowRect(handle, out info))
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
                if (Win32.GetWindowRect(handle, out info))
                    result.Add(new StickPoint(handle, info.Right, priority));
            }

            return result;
        }

        public IEnumerable<StickPoint> ForRight()
        {
            List<StickPoint> result = new List<StickPoint>();
            foreach (IntPtr handle in GetTopLevelWindows())
            {
                Win32.RECT info;
                if (Win32.GetWindowRect(handle, out info))
                    result.Add(new StickPoint(handle, info.Left, priority));
            }

            return result;
        }
    }
}
