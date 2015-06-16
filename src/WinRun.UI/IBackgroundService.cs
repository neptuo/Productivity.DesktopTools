using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun
{
    public interface IBackgroundService
    {
        void Install();
        void UnInstall();
    }
}
