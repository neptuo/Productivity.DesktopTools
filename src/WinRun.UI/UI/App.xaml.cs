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

        protected override void OnStartup(StartupEventArgs e)
        {
            CreateMainWindow();
            CreateTrayIcon();
        }

        private void CreateMainWindow()
        {
            window = new MainWindow();
            window.Show();
            Application.Current.MainWindow = window;
        }

        private void CreateTrayIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Icon = Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule.FileName);
            trayIcon.Text = "WinRun";
            trayIcon.Visible = true;

            trayIcon.ContextMenu = new ContextMenu();
            trayIcon.ContextMenu.MenuItems.Add("Clock", (sender, e) => window.ClockHandler.Handle(window.ClockHandler.LargeHotkey));
            trayIcon.ContextMenu.MenuItems.Add("Exit", (sender, e) => Shutdown());
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            trayIcon.Visible = false;
            trayIcon.Dispose();
        }
    }
}
