using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun
{
    public interface INotificationService
    {
        void ShowNotification(string title, string text, int durationMilliSeconds);
    }
}
