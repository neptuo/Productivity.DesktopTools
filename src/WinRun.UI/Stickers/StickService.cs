using Neptuo.Windows.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace WinRun.UI.Stickers
{
    public class StickService
    {
        private readonly Dispatcher dispatcher;
        private readonly Timer timer = new Timer(2000);
        private readonly HashSet<IntPtr> windows = new HashSet<IntPtr>();
        private readonly List<WindowBoundsHook> hooks = new List<WindowBoundsHook>();

        public StickService(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            timer.Elapsed += timer_Elapsed;
        }

        public const int StickOffset = 20;

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            IntPtr current = Stickers.Win32.GetForegroundWindow();
            if (current != null && !windows.Contains(current))
            {
                windows.Add(current);
                DispatcherHelper.Run(dispatcher, () => hooks.Add(new WindowBoundsHook(current, Console.WriteLine).Install()));
            }
        }

        public void Install()
        {
            timer.Start();
        }

        public void UnInstall()
        {
            foreach (var hook in hooks)
                hook.Uninstall();
        }
    }
}
