﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRun.Stickers
{
    public class DesktopStickPointProvider(int priority) : IStickPointProvider
    {
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
