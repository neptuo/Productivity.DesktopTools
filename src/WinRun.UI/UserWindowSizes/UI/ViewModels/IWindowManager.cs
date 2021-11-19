using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UserWindowSizes.UI.ViewModels
{
    public interface IWindowManager
    {
        void Update(int left, int top, int width, int height, bool isCurrentMonitor);
    }
}
