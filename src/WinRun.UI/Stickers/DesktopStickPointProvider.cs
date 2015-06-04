using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRun.UI.Stickers
{
    public class DesktopStickPointProvider : IStickPointProvider
    {
        public IEnumerable<StickInfo> ForTop()
        {
            return Screen.AllScreens.Select(s => new StickInfo(s.WorkingArea.Top));
        }

        public IEnumerable<StickInfo> ForBottom()
        {
            return Screen.AllScreens.Select(s => new StickInfo(s.WorkingArea.Bottom));
        }

        public IEnumerable<StickInfo> ForLeft()
        {
            return Screen.AllScreens.Select(s => new StickInfo(s.WorkingArea.Left));
        }

        public IEnumerable<StickInfo> ForRight()
        {
            return Screen.AllScreens.Select(s => new StickInfo(s.WorkingArea.Right));
        }
    }
}
