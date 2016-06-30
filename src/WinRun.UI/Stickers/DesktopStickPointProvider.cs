using System;
using System.Collections.Generic;
using System.Linq;
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
            return Screen.AllScreens.Select(s => new StickPoint(s.WorkingArea.Left - 3, priority));
        }

        public IEnumerable<StickPoint> ForRight()
        {
            return Screen.AllScreens.Select(s => new StickPoint(s.WorkingArea.Right, priority));
        }
    }
}
