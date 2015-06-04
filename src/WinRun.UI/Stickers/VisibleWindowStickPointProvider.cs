using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UI.Stickers
{
    public class VisibleWindowStickPointProvider : IStickPointProvider
    {
        public IEnumerable<StickInfo> ForTop()
        {
            List<StickInfo> result = new List<StickInfo>();
            foreach (IntPtr handle in Win32.GetTopLevelWindows().Where(Win32.IsWindowVisible))
            {
                Win32.RECT info;
                if (Win32.GetWindowRect(handle, out info))
                    result.Add(new StickInfo(handle, info.Bottom));
            }

            return result;
        }

        public IEnumerable<StickInfo> ForBottom()
        {
            List<StickInfo> result = new List<StickInfo>();
            foreach (IntPtr handle in Win32.GetTopLevelWindows().Where(Win32.IsWindowVisible))
            {
                Win32.RECT info;
                if (Win32.GetWindowRect(handle, out info))
                    result.Add(new StickInfo(handle, info.Top));
            }

            return result;
        }

        public IEnumerable<StickInfo> ForLeft()
        {
            List<StickInfo> result = new List<StickInfo>();
            foreach (IntPtr handle in Win32.GetTopLevelWindows().Where(Win32.IsWindowVisible))
            {
                Win32.RECT info;
                if (Win32.GetWindowRect(handle, out info))
                    result.Add(new StickInfo(handle, info.Right));
            }

            return result;
        }

        public IEnumerable<StickInfo> ForRight()
        {
            List<StickInfo> result = new List<StickInfo>();
            foreach (IntPtr handle in Win32.GetTopLevelWindows().Where(Win32.IsWindowVisible))
            {
                Win32.RECT info;
                if (Win32.GetWindowRect(handle, out info))
                    result.Add(new StickInfo(handle, info.Left));
            }

            return result;
        }
    }
}
