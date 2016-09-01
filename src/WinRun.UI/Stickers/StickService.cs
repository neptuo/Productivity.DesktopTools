using Neptuo.Windows.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using WinRun.Stickers.UI;

namespace WinRun.Stickers
{
    public class StickService : IBackgroundService
    {
        private readonly Dispatcher dispatcher;
        private readonly Timer timer = new Timer(2000);
        private readonly HashSet<IntPtr> windows = new HashSet<IntPtr>();
        private readonly List<WindowStickHook> hooks = new List<WindowStickHook>();
        private readonly IStickPointProvider pointProvider;

        public StickService(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            timer.Elapsed += OnTimer;

            pointProvider = new StickPointProviderCollection()
                .AddProvider(new DesktopStickPointProvider(1))
                .AddProvider(new VisibleWindowStickPointProvider(2));
        }

        public const int StickOffset = 20;

        public void Install()
        {
            timer.Start();

            // Here you can show window, that displays window borders.
            //StickVisualizationWindow wnd = new StickVisualizationWindow();
            //wnd.Show();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            IntPtr current = Win32.GetForegroundWindow();
            if (current != null && !windows.Contains(current))
            {
                windows.Add(current);
                DispatcherHelper.Run(dispatcher, () =>
                {
                    WindowStickHook hook = new WindowStickHook(current, pointProvider);
                    hook.Install();
                    hooks.Add(hook);
                });
            }
        }

        public void UnInstall()
        {
            foreach (var hook in hooks)
                hook.UnInstall();
        }
    }
}
