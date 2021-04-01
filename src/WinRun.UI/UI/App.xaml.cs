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
    public partial class App : Application, INotificationService
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
            window = new MainWindow(this);
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
                ShowNotification("Exception occured", e.ToString(), 5 * 1000);
        }

        public void ShowNotification(string title, string text, int durationMilliSeconds)
        {
            trayIcon.BalloonTipTitle = title;
            trayIcon.BalloonTipText = text;
            trayIcon.ShowBalloonTip(durationMilliSeconds);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            trayIcon.Visible = false;
            trayIcon.Dispose();
        }
    }
}
