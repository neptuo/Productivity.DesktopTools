using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace WinRun.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow window;
        private NotifyIcon trayIcon;

        private HelpWindow help;

        protected override void OnStartup(StartupEventArgs e)
        {
            CreateMainWindow();
            CreateTrayIcon();
        }

        private void CreateMainWindow()
        {
            window = new MainWindow();
            window.Show();

            Current.MainWindow = window;
        }

        private void CreateTrayIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Icon = Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule.FileName);
            trayIcon.Text = "WinRun";
            trayIcon.Visible = true;

            trayIcon.ContextMenu = new ContextMenu();
            trayIcon.ContextMenu.MenuItems.Add("Help", OpenHelp);
            trayIcon.ContextMenu.MenuItems.Add("Clock", (sender, e) => window.ClockHandler.Handle(window.ClockHandler.LargeHotkey));
            trayIcon.ContextMenu.MenuItems.Add("Exit", (sender, e) => Shutdown());
        }

        private void OpenHelp(object sender, EventArgs e)
        {
            if (help == null)
            {
                help = new HelpWindow();
                help.Closed += OnHelpClosed;
                help.Show();
            }
            else
            {
                help.Activate();
            }
        }

        private void OnHelpClosed(object sender, EventArgs e)
        {
            help.Closed -= OnHelpClosed;
            help = null;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            if (trayIcon != null)
            {
                trayIcon.BalloonTipTitle = "Exception occured";
                trayIcon.BalloonTipText = e.ToString();
                trayIcon.ShowBalloonTip(5 * 1000);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            trayIcon.Visible = false;
            trayIcon.Dispose();
        }
    }
}
